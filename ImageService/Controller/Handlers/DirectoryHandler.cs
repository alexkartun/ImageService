using System;
using System.IO;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Modal;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private IImageController m_controller;
        private ILoggingService m_logger;
        private FileSystemWatcher m_dirWatcher;
        public String Path { get; set; }

        public DirectoryHandler(string path, IImageController controller, ILoggingService logger)
        {
            Path = path;
            m_controller = controller;
            m_logger = logger;
            m_dirWatcher = new FileSystemWatcher(Path);
        }

        public void StartHandleDirectory()
        {
            // lookup for all extensions. - Filter in OnChanged() method.
            m_dirWatcher.Filter = "*.*";
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true;
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
                string[] args = { e.FullPath, e.Name };
				CommandRecievedEventArgs cmd_args = new CommandRecievedEventArgs
					((int) CommandEnum.NewFileCommand, e.FullPath, args);
				DoFileAction(cmd_args);
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
            m_logger.Log(result, status);
            m_controller.LogsModal.ServiceLogs.Add(new Log(result, status));
		}

        public void StopHandleDirectory()
        {
            m_dirWatcher.EnableRaisingEvents = false;
        }
    }
}
