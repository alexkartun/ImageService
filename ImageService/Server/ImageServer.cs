using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;
using System;

namespace ImageService.Server
{
	public class ImageServer
	{
		private IImageController m_controller;
		private ILoggingService logging_service;

        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        public ImageServer(IImageController controller, ILoggingService logger)
        {
            m_controller = controller;
            logging_service = logger;
        }

        /// <summary>
        /// Send command to handlers.
        /// </summary>
        /// <param name="command_args"> Command args for execution of the command. </param>
        public void SendCommand(CommandRecievedEventArgs command_args)
        {
            CommandRecieved.Invoke(this, command_args);
        }

        /// <summary>
        /// Close sender handler. Remove from events.
        /// </summary>
        /// <param name="sender"> Handler to be closed. </param>
        /// <param name="close_args"> Closing args of specific handler. </param>
        private void OnCloseServer(object sender, DirectoryCloseEventArgs close_args)
        {
            IDirectoryHandler handler = (IDirectoryHandler) sender; // TODO: check for optimize down-casting.
            CommandRecieved -= handler.OnCommandRecieved;
			//TODO: check failiure possibility.
            logging_service.Log(close_args.Message + close_args.DirectoryPath, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// Update the event logger via logger with specific message.
        /// </summary>
        /// <param name="sender"> Handler that executed the command. </param>
        /// <param name="msg_args"> Message handler passing after execution. </param>
		private void MessageLogger(object sender, MessageRecievedEventArgs msg_args)
		{
			logging_service.Log(msg_args.Message, msg_args.Status);
		}

        /// <summary>
        /// Stop The handlers.
        /// </summary>
        /// <param name="directories"> Directory paths we want to close handling. </param>
		public void StopHandlers(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {  
                SendCommand(new CommandRecievedEventArgs(
					(int) CommandEnum.CloseCommand, null, path));
			}
        }

        /// <summary>
        /// Create and start handling the directories.
        /// </summary>
        /// <param name="directories"> Directories we want to start handle. </param>
        public void CreateHandlers(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(m_controller, logging_service, path);
                CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += OnCloseServer;
				handler.DirectoryAction += MessageLogger;
				handler.StartHandleDirectory();
            }
        }
    }
}
