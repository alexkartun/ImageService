using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Model;
using ImageService.Model.Event;

namespace ImageService.Model
{
	public interface ICloseModal
	{
		event EventHandler<DirectoryCloseEventArgs> OnClose;
		string CloseDirectory(string[] args, out MessageTypeEnum status);
	}
}
