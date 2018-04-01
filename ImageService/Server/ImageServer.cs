using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Modal;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        private IImageController m_controller;
        private List<IDirectoryHandler> directory_handlers;

        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        public ImageServer(IImageController controller)
        {
            m_controller = controller;
            directory_handlers = new List<IDirectoryHandler>();
        }

        public void OnAlarmEvent(CommandRecievedEventArgs e)
        {
            CommandRecieved(this, e);
        }

        public void CloseHandler(DirectoryCloseEventArgs e)
        {
            foreach (IDirectoryHandler handler in directory_handlers)
            {
                
            }
        }

        // Possible "OnStart" service method.
        public void StartHandling(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(m_controller);
                directory_handlers.Add(handler);
                CommandRecieved += handler.OnCommandRecieved;
                handler.StartHandleDirectory(path);
            }
        }
    }
}
