using System;
using System.IO;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using ImageService.Infastructure.Model;
using ImageService.Logging.Model;

namespace ImageService.Controller.Handlers
{
    class DirectoryHandler : IDirectoryHandler
    {
        private FileSystemWatcher m_dirWatcher;
        public String Path { get; set; }

        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        public DirectoryHandler(string path)
        {
            Path = path;
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
                CommandRecieved(this, new CommandRecievedEventArgs((int) CommandEnum.NewFileCommand, args));
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

        public void StopHandleDirectory()
        {
            m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose();
        }
    }
}
