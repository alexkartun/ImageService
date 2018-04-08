﻿using System;

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
