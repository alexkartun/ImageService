using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class TcpServerChannel
    {
        private string ip;
        private string port;
        private TcpListener server;
        private List<TcpClient> clients;
        private ClientHandler ch;
        private IImageController controller;

        public TcpServerChannel(string ip, string port, IImageController controller)
        {
            this.ip = ip;
            this.port = port;
            clients = new List<TcpClient>();
            ch = new ClientHandler();
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            server = new TcpListener(ep);
            Task task = new Task(() =>
            {
                server.Start();
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }

                }
            });
            task.Start();
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}
