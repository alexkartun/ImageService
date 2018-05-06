using ImageService.Communication.Model;
using ImageService.Infastructure.Event;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
        public ClientHandler Client_Handler { get; }

        public TcpServerChannel(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
            clients = new List<TcpClient>();
            Client_Handler = new ClientHandler();
        }

        public void SendCommandBroadCast(CommandMessage msg)
        {
            string output = JsonConvert.SerializeObject(msg);
            foreach (TcpClient client in clients)
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(output);
                }
            }
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
                        Client_Handler.HandleClient(client);
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
