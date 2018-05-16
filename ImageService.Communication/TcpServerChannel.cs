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

        private ClientHandler handler;
        public ClientHandler Client_Handler { get { return handler;  } }

        public TcpServerChannel()
        {
            clients = new List<TcpClient>();
            handler = new ClientHandler();
            handler.ExitRecieved += OnExitRecieved;
        }

        public void OnExitRecieved(object sender, CommandRecievedEventArgs args)
        {
            clients.Remove(args.Client_Socket);
            args.Client_Socket.Close();
        }

        public void SendCommandBroadCast(CommandMessage msg)
        {
            try
            {
                string output = JsonConvert.SerializeObject(msg);
                foreach (TcpClient client in clients)
                {
                    NetworkStream stream = client.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(output);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                        handler.HandleClient(client);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            });
            task.Start();
        }

        public void Stop()
        {
            // Send for every client to exit.
            CommandMessage msg = new CommandMessage((int)CommandEnum.ExitCommand);
            SendCommandBroadCast(msg);

            foreach (TcpClient client in clients)
            {
                client.Close();
            }

            server.Stop();
        }
    }
}
