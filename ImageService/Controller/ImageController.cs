using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using ImageService.Infastructure.Enums;
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
                // The only command in the map for now. New file command.
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(m_modal) }
            };
        }

		public string ExecuteCommand(int commandID, string[] args, out bool status)
		{
			ICommand command = commands[commandID];
            // Create task for every command that we want to execute. 
            Task<Tuple<string, bool>> task = Task.Run<Tuple<string, bool>>(() => 
                { return CommandAction(command, args); });
            // Father wait for son to finish and get the result after execution.
            task.Wait();
            // Update the values and return.
            var value = task.Result;
            status = value.Item2;
			return value.Item1;
        }

        /// <summary>
        /// Command executed after task/thread created.
        /// </summary>
        /// <param name="command"> Command to execute. </param>
        /// <param name="args"> Arguments of the command. </param>
        /// <returns> Return tuple type including 2 items: string representation and bollean status of execution. 
        /// </returns>
        private Tuple<string, bool> CommandAction(ICommand command, string[] args)
        {
            bool status;
            string result = command.Execute(args, out status);
            return new Tuple<string, bool>(result, status);
        }
	}
}