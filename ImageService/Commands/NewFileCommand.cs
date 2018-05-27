using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

		/// <summary>
		/// newfile command constructor. ref to image service modal is given.
		/// </summary>
		public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        /// <summary>
        /// New file command execution. Add file called.
        /// </summary>
        public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            return m_modal.AddFile(args, out result);
        }
    }
}
