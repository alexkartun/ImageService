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
            m_dirWatcher.EnableRaisingEvents = false;
            // dirWatcher is no longer listening.
            m_dirWatcher.Dispose();
            string msg = "Closing directory successfully: ";
            DirectoryCloseEventArgs close_args = new DirectoryCloseEventArgs(m_path, msg);
            DirectoryClose.Invoke(this, close_args);
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
			m_dirWatcher = new FileSystemWatcher(m_path);
            // lookup for all extensions. - Filter in OnChanged() method.
            m_dirWatcher.Filter = "*.*";
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// When image was created on directory. Check the extanssion of the image.
        /// Build command args for new file command and send to cotroller for execution.
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
			bool status;
			string result = m_controller.ExecuteCommand(args.CommandID,
				args.Args, out status);
            // status = false/failure
			if (!status)
			{
				MessageRecievedEventArgs msg_args =
					new MessageRecievedEventArgs(result, MessageTypeEnum.FAIL);
				DirectoryAction.Invoke(this, msg_args);
			}
            // otherwise,
			else
			{
				MessageRecievedEventArgs msg_args =
					new MessageRecievedEventArgs(result, MessageTypeEnum.INFO);
				DirectoryAction.Invoke(this, msg_args);
			}
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
