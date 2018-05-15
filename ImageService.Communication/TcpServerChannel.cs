using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class TcpServerChannel
    {
        private static string ip = "127.0.0.1";
        private static string port = "8000";
        private TcpListener server;
        private List<TcpClient> clients;
        public ClientHandler Client_Handler { get; }

        public TcpServerChannel()
        {
            clients = new List<TcpClient>();
            Client_Handler = new ClientHandler();
            Client_Handler.ExitRecieved += OnExitRecieved;
        }

        public void OnExitRecieved(object sender, CommandRecievedEventArgs args)
        {
            clients.Remove(args.Client_Socket);
        }

        public void SendCommandBroadCast(CommandMessage msg)
        {
            string output = JsonConvert.SerializeObject(msg);
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream)
                {
                    AutoFlush = true
                };
                writer.WriteLine(output);
            }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            server = new TcpListener(ep);
            server.Start();
            Task task = new Task(() =>
            {
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
            // Send for every client to exit.
            foreach (TcpClient client in clients)
            {
                // Service stopping
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream)
                {
                    AutoFlush = true
                };
                CommandMessage msg = new CommandMessage((int)CommandEnum.ExitCommand);
                string output = JsonConvert.SerializeObject(msg);
                writer.WriteLine(output);
                client.GetStream().Close(); // Close the stream
                client.Close(); // Close the client
            }
            server.Stop();
        }
    }
}
