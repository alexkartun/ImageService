using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        private ICloseModal m_modal;

        public CloseCommand(ICloseModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
			return m_modal.CloseDirectory(args, out result);
        }
    }
}
