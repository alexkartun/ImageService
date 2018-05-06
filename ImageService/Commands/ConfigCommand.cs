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
    class ConfigCommand : ICommand
    {
        private ISettingsModal m_modal;

        public ConfigCommand(ISettingsModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result)
        {
            return m_modal.GetConfig(out result);
        }
    }
}
