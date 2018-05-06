using ImageService.Communication;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Model.Event;
using System.Collections.Generic;
using System.Configuration;

namespace ImageService.Server
{
    class ImageServer : IServer
    {
        private ILoggingService logging_service;
        private IImageController image_controller;
        private List<IDirectoryHandler> directory_handlers;
        private TcpServerChannel server;

        public ImageServer(string ip, string port, ILoggingService logger, IImageController controller)
        {
            server = new TcpServerChannel(ip, port, controller);
            image_controller = controller;
            logging_service = logger;
            directory_handlers = new List<IDirectoryHandler>();
        }

        public void Start()
        {
            CreateHandlers();
            server.Start();
        }

        public void Stop()
        {
            StopHandlers();
            server.Stop();
        }

        /// <summary>
        /// Close sender handler. Remove from events.
        /// </summary>
        /// <param name="sender"> Handler to be closed. </param>
        /// <param name="close_args"> Closing args of specific handler. </param>
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs close_args)
        {
            foreach (IDirectoryHandler dir in directory_handlers)
            {
                if (dir.Path.CompareTo(close_args.DirectoryPath) == 0)
                {
                    dir.StopHandleDirectory();
                    break;
                }
            }
            // Remove the directory from the handlers.
            directory_handlers.RemoveAll(dir => dir.Path == close_args.DirectoryPath);
            // Remove the path from the config.
            image_controller.SettingsModal.ServiceConfig.Directory_Paths.Remove(close_args.DirectoryPath);
            // Update all clients.
        }

        /// <summary>
        /// Stop The handlers.
        /// </summary>
        /// <param name="directories"> Directory paths we want to close handling. </param>
		private void StopHandlers()
        {
            foreach (IDirectoryHandler directory in directory_handlers)
            {
                directory.StopHandleDirectory();
            }
            //TODO: Send to clients that service stopped. Close all clients sockets.
        }

        /// <summary>
        /// Create and start handling the directories.
        /// </summary>
        private void CreateHandlers()
        {
            string directories = ConfigurationManager.AppSettings["Handler"];
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(path, image_controller, logging_service);
                directory_handlers.Add(handler);
                handler.StartHandleDirectory();
            }
        }
    }
}
