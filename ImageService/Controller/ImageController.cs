using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using ImageService.Infastructure.Enums;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using System.Net.Sockets;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        public IImageServiceModal ImageModal { get; set; }
        private Dictionary<int, ICommand> commands;

		public ImageController(IImageServiceModal modal)
        {
            ImageModal = modal;
            commands = new Dictionary<int, ICommand>()
            {
                // The only command in the map for now. New file command.
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(ImageModal) },
                { (int) CommandEnum.CloseCommand, new CloseCommand(ImageModal) },
                { (int) CommandEnum.LogCommand, new LogCommand(ImageModal) },
                { (int) CommandEnum.GetConfigCommand, new ConfigCommand(ImageModal) }
            };
        }

		public string ExecuteCommand(int commandID, string[] args, out MessageTypeEnum status, TcpClient client = null)
		{
			ICommand command = commands[commandID];
            return command.Execute(args, out status, client);
        }
	}
}