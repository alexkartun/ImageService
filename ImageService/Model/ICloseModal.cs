using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;
using ImageService.Model.Event;

namespace ImageService.Model
{

	public interface ICloseModal
	{
		event EventHandler<DirectoryCloseEventArgs> OnClose;
		/// <summary>
		/// Closes one directory handler.
		/// </summary>
		/// <param name="args"> Args of the command including name and path of handler. </param>
		/// <param name="status"> Sets to INFO unless an error occured. </param>
		/// <returns> Return exception message if was throwed or success message. </returns>
		string CloseDirectory(string[] args, out MessageTypeEnum status);
    }
}
