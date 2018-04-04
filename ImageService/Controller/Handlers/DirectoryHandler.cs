using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageService.Infastructure;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;
using ImageService.Server;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private IImageController m_controller;
        private FileSystemWatcher m_dirWatcher;
        private string m_path;

		public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
		public event EventHandler<MessageRecievedEventArgs> DirectoryAction; // Standart commands (AddFile etc.)

		public DirectoryHandler(IImageController controller, ILoggingService logger, string path)
        {
            m_path = path;
            m_controller = controller;
        }

        public void CloseHandler()
        {
            m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose(); // dirWatcher is no longer listening.
            string msg = "Closing directory successfully: "; // TODO: when is not successfully??
            DirectoryCloseEventArgs close_args = new DirectoryCloseEventArgs(m_path, msg);
            DirectoryClose.Invoke(this, close_args);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs command_args)
        {
            if (command_args.RequestDirPath.CompareTo(m_path) == 0)
            {
                if (command_args.CommandID == (int)CommandEnum.CloseCommand)
                {
                    CloseHandler();
                }
                else
                {
					DoFileAction(command_args);
				}
            }
        }

        public void StartHandleDirectory()
        {
			m_dirWatcher = new FileSystemWatcher(m_path);
            m_dirWatcher.Filter = "*.*";    // lookup for all extensions. - Filter in OnChanged() method.
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true; // check.
        }

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

		private void DoFileAction(CommandRecievedEventArgs args)
		{
			bool status;
			string result = m_controller.ExecuteCommand(args.CommandID,
				args.Args, out status);
			if (!status)
			{
				MessageRecievedEventArgs msg_args =
					new MessageRecievedEventArgs(result, MessageTypeEnum.FAIL);
				DirectoryAction.Invoke(this, msg_args);
			}
			else
			{
				MessageRecievedEventArgs msg_args =
					new MessageRecievedEventArgs(result, MessageTypeEnum.INFO);
				DirectoryAction.Invoke(this, msg_args);
			}
		}


        private static string GetFileExt(string filePath)
        {
            if (filePath == null) return "";
            if (filePath.Length == 0) return "";
            if (filePath.LastIndexOf(".") == -1) return "";
            return filePath.Substring(filePath.LastIndexOf("."));
        }
    }
}
