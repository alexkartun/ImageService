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
            Task<Tuple<string, bool>> task = Task.Run<Tuple<string, bool>>(() => 
                { return GetDataTask(command, args); });
            task.Wait();
            var value = task.Result;
            status = value.Item2;
			return value.Item1;
        }

        private Tuple<string, bool> GetDataTask(ICommand command, string[] args)
        {
            bool status;
            string result = command.Execute(args, out status);
            return new Tuple<string, bool>(result, status);
        }
	}
}