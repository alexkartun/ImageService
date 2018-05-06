using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private ILogsServiceModal m_modal;

        public LogCommand(ILogsServiceModal modal)
        {
            m_modal = modal;
        }
     
        public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            return m_modal.GetAllLog(out result, client);
        }
    }
}
