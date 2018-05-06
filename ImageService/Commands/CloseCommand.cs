using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public CloseCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            result = MessageTypeEnum.INFO;
            return "";
        }
    }
}
