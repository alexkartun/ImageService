using System;

namespace ImageService.Model.Event
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
