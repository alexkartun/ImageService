using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private ILogsServiceModal m_modal;

		/// <summary>
		/// log command constructor. ref to log service modal is given.
		/// </summary>
		public LogCommand(ILogsServiceModal modal)
        {
            m_modal = modal;
        }

		/// <summary>
		/// log command execution. A method which summons all logs via TCP is called.
		/// </summary>
		public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            return m_modal.GetAllLog(out result, client);
        }
    }
}
