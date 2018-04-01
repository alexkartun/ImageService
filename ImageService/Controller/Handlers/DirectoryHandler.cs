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
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private IImageController m_controller;
        private ILoggingService m_logger;
        private FileSystemWatcher m_dirWatcher;
        private string m_path;

        public DirectoryHandler(IImageController controller, ILoggingService logger, string path)
        {
            m_path = path;
            m_logger = logger;
            m_controller = controller;
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public void CloseHandler()
        {
            m_dirWatcher.EnableRaisingEvents = false;  //TODO: check if need to write this method.
            m_dirWatcher.Dispose();
            string msg = "Closing Directory!";    //TODO:WHAT message should we pass.
            DirectoryCloseEventArgs close_args = new DirectoryCloseEventArgs(m_path, msg);
            DirectoryClose.Invoke(this, close_args);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs command_args)
        {
            if (command_args.RequestDirPath.CompareTo(m_path) == 0)
            {
                if (command_args.CommandID == (int) CommandEnum.CloseCommand)
                {
                    CloseHandler();
                }
                m_controller.ExecuteCommand(command_args.CommandID, command_args.Args);
                //m_logger.Log();    //TODO: update the message to logger.
            }
        }

        public void StartHandleDirectory()
        {
            m_dirWatcher = new FileSystemWatcher(m_path)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName
            };
            m_dirWatcher.Filter = "*.*";    // lookup for all extensions.
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string strFileExt = GetFileExt(e.FullPath);
            if (strFileExt.CompareTo(".jpg") == 0 || strFileExt.CompareTo(".png") == 0
                || strFileExt.CompareTo(".gif") == 0 || strFileExt.CompareTo(".bmp") == 0)
            {
                string[] args = new string[1];   //TODO:What args should we pass.
                args[0] = e.FullPath;
                m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args); //TODO:try catch, message.
                //m_logger.Log();    //TODO: update the message to logger.
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
