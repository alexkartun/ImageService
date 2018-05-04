using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }

        public CommandRecievedEventArgs(int id, string path = null, string[] args = null)
        {
            CommandID = id;
            RequestDirPath = path;
            Args = args;
        }
    }
}
