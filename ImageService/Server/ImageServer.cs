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
            IDirectoryHandler handler = (IDirectoryHandler) sender;
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= OnCloseServer;
            logging_service.Log(close_args.Message, MessageTypeEnum.INFO);    //TODO:Check typeEnum, maybe for future events.
        }

        // Possible "OnStop" service method.
        public void StopHandlers(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                CommandRecievedEventArgs command_args = new CommandRecievedEventArgs(
                    (int) CommandEnum.CloseCommand, null, path);    //TODO:null for args, is it the way?
                SendCommand(command_args);
            }
        }

        // Possible "OnStart" service method.
        public void CreateHandlers(string directories)
        {
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(m_controller, logging_service, path);
                CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += OnCloseServer;
                handler.StartHandleDirectory();
            }
        }
    }
}
