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
        public ISettingsModal SettingsModal { get; set; }
        public ILogsServiceModal LogsModal { get; set; }
        private Dictionary<int, ICommand> commands;

		public ImageController(IImageServiceModal image_modal, ILogsServiceModal logging_modal,
            ISettingsModal settings_modal)
        {
            ImageModal = image_modal;
            LogsModal = logging_modal;
            SettingsModal = settings_modal;
            commands = new Dictionary<int, ICommand>()
            {
                // The only command in the map for now. New file command.
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(ImageModal) },
                //{ (int) CommandEnum.CloseCommand, new CloseCommand(ImageModal) },
                { (int) CommandEnum.LogCommand, new LogCommand(LogsModal) },
                { (int) CommandEnum.GetConfigCommand, new ConfigCommand(SettingsModal) }
            };
        }

		public string ExecuteCommand(int commandID, string[] args, out MessageTypeEnum status)
		{
			ICommand command = commands[commandID];
            return command.Execute(args, out status);
        }
	}
}