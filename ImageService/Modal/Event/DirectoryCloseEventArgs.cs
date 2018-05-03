using System;

namespace ImageService.Modal.Event
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }

        public DirectoryCloseEventArgs(string dirPath)
        {
            DirectoryPath = dirPath;
        }
    }
}
