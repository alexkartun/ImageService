using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model
{
    public class CommandMessage
    {
        public int Command { get; }
        public string[] Args { get; }

        public CommandMessage(int a_cmd, string[] a_args)
        {
            Command = a_cmd;
            Args = a_args;
        }
    }
}
