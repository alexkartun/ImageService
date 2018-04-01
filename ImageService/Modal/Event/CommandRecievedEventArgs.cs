using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }

        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
