using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using ImageService.Infastructure.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(m_modal) }
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            ICommand command = commands[commandID];
            return command.Execute(args, out result);
        }
    }
}