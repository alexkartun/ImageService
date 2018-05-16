using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Converters
{
    public class CommandConverter
    {
        public event EventHandler<CommandRecievedEventArgs> SettingsRecieved;
        public event EventHandler<CommandRecievedEventArgs> LogsRecieved;

        public Boolean Convert(CommandMessage msg)
        {
            if (msg.Command == (int) CommandEnum.GetConfigCommand ||
                msg.Command == (int) CommandEnum.CloseCommand)
            {
                SettingsRecieved(this, new CommandRecievedEventArgs(msg.Command, msg.Args));
            }
            else if (msg.Command == (int) CommandEnum.LogCommand)
            {
                LogsRecieved(this, new CommandRecievedEventArgs(msg.Command, msg.Args));
            }
            else // Exit recieved from service.
            {
                return false;
            }
            return true;
        }
    }
}
