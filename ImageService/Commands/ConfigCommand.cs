using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class ConfigCommand : ICommand
    {
        private ISettingsModal m_modal;
		/// <summary>
		/// configuration command constructor. ref to setting modal is given.
		/// </summary>
		public ConfigCommand(ISettingsModal modal)
        {
            m_modal = modal;
        }

		/// <summary>
		/// Config command execution. "GetSetting" method from modal is called.
		/// </summary>
		public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            return m_modal.GetSettings(out result, client);
        }
    }
}
