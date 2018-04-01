using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageService.Infastructure;
using ImageService.Infastructure.Enums;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private IImageController m_controller;
        private FileSystemWatcher m_dirWatcher;
        private string m_path;

        public DirectoryHandler(IImageController controller)
        {
            m_controller = controller;
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.CompareTo(m_path) == 0)
            {
                Console.WriteLine(e.RequestDirPath);
                /*
                bool result;
                string status = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (!result)
                {
                    Console.WriteLine(status);
                }
                */
            }
            else
            {
                Console.WriteLine("Its not me");
            }
        }

        public void OnCloseRecieved(object sender, DirectoryCloseEventArgs e)
        {
            if (e.DirectoryPath.CompareTo(m_path) == 0)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Close(DirectoryCloseEventArgs e)
        {
            DirectoryClose(this, e);
        }

        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            DirectoryClose += OnCloseRecieved;
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
                bool result;
                string[] args = new string[1];
                args[0] = e.FullPath;
                string status = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
                if (!result)
                {
                    Console.WriteLine(status);
                }
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
