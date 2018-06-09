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
    public class TcpServerChannelWithWeb
    {
        private static string ip = "127.0.0.1";
        private static string port = "8001";
        private TcpListener server;
        private List<TcpClient> clients;
        public ClientHandlerWithWeb ClientHandler { get; set; }

        public TcpServerChannelWithWeb()
        {
            clients = new List<TcpClient>();
            ClientHandler = new ClientHandlerWithWeb();
        }

        public void Write(TcpClient client, CommandMessage m)
        {
            string output = JsonConvert.SerializeObject(m);
            try
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(output);
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Start()
        {
            // Server end-point declaration.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            server = new TcpListener(ep);
            server.Start();
            // Creates all handlers and listen to all clients.
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);
                        ClientHandler.HandleClient(client);
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
            server.Stop();
            foreach (TcpClient client in clients)
            {
                try
                {
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
