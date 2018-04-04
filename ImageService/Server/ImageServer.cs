using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
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
		private ILoggingService logging_service;

        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        public ImageServer(IImageController controller, ILoggingService logger)
        {
            m_controller = controller;
            logging_service = logger;
        }

        public void SendCommand(CommandRecievedEventArgs command_args)
        {

            CommandRecieved.Invoke(this, command_args);
        }

        private void OnCloseServer(object sender, DirectoryCloseEventArgs close_args)
        {
            IDirectoryHandler handler = (IDirectoryHandler) sender; // TODO: check for optimize down-casting.
            CommandRecieved -= handler.OnCommandRecieved;
			//TODO: check failiure possibility.
            logging_service.Log(close_args.Message + close_args.DirectoryPath, MessageTypeEnum.INFO);
        }

		private void MessageLogger(object sender, MessageRecievedEventArgs msg_args)
		{
			logging_service.Log(msg_args.Message, msg_args.Status);
		}

		public void StopHandlers(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {  
                SendCommand(new CommandRecievedEventArgs(
					(int) CommandEnum.CloseCommand, null, path));
			}
        }

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
