using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        void StartHandleDirectory(string dirPath);
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
        void OnCloseRecieved(object sender, DirectoryCloseEventArgs e);
    }
}