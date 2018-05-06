using ImageService.Logging.Model;
using ImageService.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    class ConfigCommand : ICommand
    {
        private ISettingsModal m_modal;

        public ConfigCommand(ISettingsModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null)
        {
            return m_modal.GetConfig(out result, client);
        }
    }
}
