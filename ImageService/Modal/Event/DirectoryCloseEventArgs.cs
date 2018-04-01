using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }
        public string Message { get; set; }

        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;
            Message = message;
        }
    }
}
