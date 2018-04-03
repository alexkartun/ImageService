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

		delegate string commandFunc(string[] args, out bool result);

		public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(m_modal) }
            };
        }

		public string ExecuteCommand(int commandID, string[] args, out bool status)
		{
			ICommand command = commands[commandID];
			commandFunc cmd = new commandFunc(command.Execute);
			Task<string> t = new Task<string>(() => cmd(args, out status));
			

				/*Task t = Task.Run(() =>
				{
				string outMessage = command.Execute(args, out result);
				}	*/
				//TODO: check if it is correct. check systax.
		    t.Wait(); // check why.
			return t.Result;
        }
	}
}