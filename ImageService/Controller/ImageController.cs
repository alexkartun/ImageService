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
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;

		public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;
            commands = new Dictionary<int, ICommand>()
            {
                // The only command in the map for now. New file command.
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(m_modal) },
                { (int) CommandEnum.CloseCommand, new CloseCommand(m_modal) },
                { (int) CommandEnum.LogCommand, new LogCommand(m_modal) },
                { (int) CommandEnum.GetConfigCommand, new ConfigCommand(m_modal) }
            };
        }

		public string ExecuteCommand(int commandID, string[] args, out MessageTypeEnum status, TcpClient client = null)
		{
			ICommand command = commands[commandID];
            return command.Execute(args, out status, client);
        }
	}
}