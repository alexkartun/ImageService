using ImageService.Commands;
using System.Collections.Generic;
using ImageService.Infastructure.Enums;
using ImageService.Model;
using ImageService.Logging.Model;
using System.Net.Sockets;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        public IImageServiceModal ImageModal { get; set; }
        public ISettingsModal SettingsModal { get; set; }
        public ILogsServiceModal LogsModal { get; set; }
		public ICloseModal CloseModal { get; set; }

		private Dictionary<int, ICommand> commands;

		public ImageController(IImageServiceModal image_modal, ILogsServiceModal logging_modal,
            ISettingsModal settings_modal, ICloseModal close_modal)
        {
            ImageModal = image_modal;
            LogsModal = logging_modal;
            SettingsModal = settings_modal;
			CloseModal = close_modal;

			/// Creates an enum-ICommand dictionary. 
			commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(ImageModal) },
                { (int) CommandEnum.CloseCommand, new CloseCommand(CloseModal) },
                { (int) CommandEnum.LogCommand, new LogCommand(LogsModal) },
                { (int) CommandEnum.GetConfigCommand, new ConfigCommand(SettingsModal) }
                
            };
        }
		/// <summary>
		/// Convert commandID to ICommand type via "commands" dictionary.
		/// Executes this command.
		/// </summary>
		public string ExecuteCommand(int commandID, string[] args, out MessageTypeEnum status, TcpClient client)
		{
			ICommand command = commands[commandID];
            return command.Execute(args, out status, client);
        }
	}
}