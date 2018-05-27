using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;
using ImageService.Model.Event;

namespace ImageService.Model
{
	public class CloseModal : ICloseModal
	{
		public event EventHandler<DirectoryCloseEventArgs> OnClose;

		/// <summary>
		/// Closes given directory (directory path is in args).
		/// Sends a notification to server which holds a list of all handlers (closes only given one).
		/// </summary>
		public string CloseDirectory(string[] args, out MessageTypeEnum status)
		{
			status = MessageTypeEnum.INFO;
			OnClose(this, new DirectoryCloseEventArgs(args[0]));
            return "Got command: " + (CommandEnum.CloseCommand).ToString() + " Args: " + args[0];
        }
    }
}
