using ImageService.Communication;
using ImageService.Communication.Model;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using ImageService.Logging;
using ImageService.Logging.Model;
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

        public ImageServer(ILoggingService logger, IImageController controller)
        {
            server = new TcpServerChannel();
            image_controller = controller;
            logging_service = logger;
            directory_handlers = new List<IDirectoryHandler>();
        }

        public void Start()
        {
            CreateHandlers();
            // Bind event to handler client component.
            server.Client_Handler.CommandRecieved += OnCommandRecieved;
            server.Start();
        }

        public void Stop()
        {
            StopHandlers();
            server.Stop();
        }

		private void OnCommandRecieved(object sender, CommandRecievedEventArgs c_args)
		{
            string output = image_controller.ExecuteCommand(c_args.Command, c_args.Args, out MessageTypeEnum status,
				c_args.Client_Socket);
			// Update eventlogger and LogsModal with new log.
			logging_service.Log(output, status);
            string[] args = { output, ((int)status).ToString() };
            image_controller.LogsModal.ServiceLogs.Add(args[0]);
            image_controller.LogsModal.ServiceLogs.Add(args[1]);
            // Send log to all clients.
            server.SendCommandBroadCast(new CommandMessage((int) CommandEnum.LogCommand, args));
            
		}
			
        /// <summary>
        /// Close sender handler. Remove from events.
        /// </summary>
        /// <param name="sender"> Handler to be closed. </param>
        /// <param name="close_args"> Closing args of specific handler. </param>
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs close_args)
        {
			IDirectoryHandler rm_dir = null;
			foreach (IDirectoryHandler dir in directory_handlers)
            {
                if (dir.Path.CompareTo(close_args.DirectoryPath) == 0)
                {
                    dir.StopHandleDirectory();
					rm_dir = dir;
					break;
                }
            }
			// Remove the directory from the handlers.
			directory_handlers.Remove(rm_dir);
            // Remove the path from the config.
            image_controller.SettingsModal.Directory_Paths.Remove(close_args.DirectoryPath);
            // Update all clients.
            string[] args = { close_args.DirectoryPath };
            server.SendCommandBroadCast(new CommandMessage((int)CommandEnum.CloseCommand, args));
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
            // Update all clients that server is closing.
            server.SendCommandBroadCast(new CommandMessage((int)CommandEnum.ExitCommand));
			//TODO: check observeable pattern.
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
                IDirectoryHandler handler = new DirectoryHandler(path);
                directory_handlers.Add(handler);
                handler.StartHandleDirectory();
                // Bind handler new command event with server.
                handler.CommandRecieved += OnCommandRecieved;
                // Add directory path to Config.
                image_controller.SettingsModal.Directory_Paths.Add(path);
            }
			image_controller.CloseModal.OnClose += OnCloseHandler;
		}
    }
}
