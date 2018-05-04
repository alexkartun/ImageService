using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
{
    public class LogEventArgs : EventArgs
    {
        public TcpClient CLIENT { get; set; }
        public LogEventArgs(TcpClient client)
        {
            CLIENT = client;
        }
    }
}
