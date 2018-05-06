using System;
using System.Net.Sockets;

namespace ImageService.Infastructure.Event
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int Command { get; }
        public string[] Args { get; }
        public TcpClient Client_Socket { get; }

        public CommandRecievedEventArgs(int a_id, string[] a_args, TcpClient client = null)
        {
            Command = a_id;
            Args = a_args;
            Client_Socket = client;
        }
    }
}
