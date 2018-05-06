using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model
{
    public class CommandMessage
    {
        public int Command { get; set; }
        public string Json_Ser_Data { get; set; }

        public CommandMessage(int cmd, string json_ser)
        {
            Command = cmd;
            Json_Ser_Data = json_ser;
        }
    }
}
