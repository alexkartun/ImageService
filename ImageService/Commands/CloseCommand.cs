using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            return m_modal.CloseDirectory(args, out result);
        }
    }
}
