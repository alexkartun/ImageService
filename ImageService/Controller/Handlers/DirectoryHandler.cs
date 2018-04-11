using System;
using System.IO;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private IImageController m_controller;
        private FileSystemWatcher m_dirWatcher;
        private string m_path;

		public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // Standart commands (AddFile etc.)
        public event EventHandler<MessageRecievedEventArgs> DirectoryAction;

		public DirectoryHandler(IImageController controller, ILoggingService logger, string path)
        {
            m_path = path;
            m_controller = controller;
        }

        /// <summary>
        /// Close handler. Invoke directory close event.
        /// </summary>
        private void CloseHandler()
        {
            string msg = "Closing directory successfully: ";
            try
            {
                m_dirWatcher.EnableRaisingEvents = false;
                // dirWatcher is no longer listening.
                m_dirWatcher.Dispose();
            } 
            catch(Exception e)
            {
                msg = "Error occured closing handler: " + m_path + ". Details: " + e.Data.ToString();
            }
            DirectoryCloseEventArgs close_args = new DirectoryCloseEventArgs(m_path, msg);
            DirectoryClose(this, close_args);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs command_args)
        {
            // Checks if handler's path is similar to one that was passed.
            if (command_args.RequestDirPath.CompareTo(m_path) == 0)
            {
                // Close_command.
                if (command_args.CommandID == (int)CommandEnum.CloseCommand)
                {
                    CloseHandler();
                }
                // Otherwise,
                else
                {
					DoFileAction(command_args);
				}
            }
        }

        public void StartHandleDirectory()
        {
            string msg = "Start handling directory: " + m_path;
            try
            {
                m_dirWatcher = new FileSystemWatcher(m_path);
            }
            catch (Exception e)
            {
                // Update logger about failure on starting handling.
                msg = "Error occured starting handler: " + m_path + ". Details: " + e.Data.ToString();
                DirectoryCloseEventArgs close_args = new DirectoryCloseEventArgs(m_path, msg);
                DirectoryClose(this, close_args);
                return;
            }
            // lookup for all extensions. - Filter in OnChanged() method.
            m_dirWatcher.Filter = "*.*";
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true;
            // Update logger about starting handling.
            MessageRecievedEventArgs msg_args =
                    new MessageRecievedEventArgs(msg, MessageTypeEnum.INFO);
            DirectoryAction(this, msg_args);
        }

        /// <summary>
        /// Checking if the current file is already in use.
        /// </summary>
        /// <param name="file"> File to be checked. </param>
        /// <returns> Return true if in use and false otherwise. </returns>
        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }

        /// <summary>
        /// When image was created on directory. Check the extanssion of the image.
        /// Build command args for new file command and send to cotroller for execution.
        /// Checks if the file is in use already.
        /// </summary>
        /// <param name="source"> Object sender. </param>
        /// <param name="e"> File system event arguments. </param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string strFileExt = GetFileExt(e.FullPath);
			// Filter for pictures only.
            if (strFileExt.CompareTo(".jpg") == 0 || strFileExt.CompareTo(".png") == 0
                || strFileExt.CompareTo(".gif") == 0 || strFileExt.CompareTo(".bmp") == 0)
            {
                // Checks if file is already in use.
                if (IsFileLocked(new FileInfo(e.FullPath)))
                {
                    return;
                }
                string[] args = { e.FullPath, e.Name };
				CommandRecievedEventArgs cmd_args = new CommandRecievedEventArgs
					((int) CommandEnum.NewFileCommand, args, null);
				DoFileAction(cmd_args);
            }
        }

        /// <summary>
        /// Execute the command via controller. Get status and result of the execution. And update the event logger via
        /// directory_action event that invoke server function for updating the logger.
        /// </summary>
        /// <param name="args"> Arguments of command. </param>
		private void DoFileAction(CommandRecievedEventArgs args)
		{
			MessageTypeEnum status;
			string result = m_controller.ExecuteCommand(args.CommandID,
				args.Args, out status);

			MessageRecievedEventArgs msg_args =
					new MessageRecievedEventArgs(result, status);
			DirectoryAction(this, msg_args);
		}

        /// <summary>
        /// Static function for getting extenssion of the file that was created on the directory.
        /// </summary>
        /// <param name="filePath"> Path of the file. </param>
        /// <returns> Returns string representation of the extenssion. </returns>
        private static string GetFileExt(string filePath)
        {
            if (filePath == null) return "";
            if (filePath.Length == 0) return "";
            if (filePath.LastIndexOf(".") == -1) return "";
            return filePath.Substring(filePath.LastIndexOf("."));
        }
    }
}
