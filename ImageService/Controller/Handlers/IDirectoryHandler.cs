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
        void StartHandleDirectory();
        void OnCommandRecieved(object sender, CommandRecievedEventArgs command_args);
    }
}