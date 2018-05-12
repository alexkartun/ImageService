using ImageService.Communication.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class TcpClientChannel
    {
        private static TcpClientChannel instance = null;
        private static string ip = "127.0.0.1";
        private static string port = "8000";
        private TcpClient client;

        private TcpClientChannel()
        {
            client = new TcpClient();
        }
        public static TcpClientChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpClientChannel();
                }
                return instance;
            }
        }

        public Boolean Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            client.Connect(ep);
            return client.Connected;
        }

        public void SendMessage(CommandMessage msg)
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            string input = JsonConvert.SerializeObject(msg);
            writer.WriteLine(input);
        }

        public CommandMessage GetMessage()
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            string output = reader.ReadLine();
            CommandMessage msg = JsonConvert.DeserializeObject<CommandMessage>(output);
            return msg;
        }
    }
}
