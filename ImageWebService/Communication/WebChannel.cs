using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWebService.Communication
{
    // GUI as a singleton.
    public class WebChannel
    {
        private static string ip = "127.0.0.1";
        private static string port = "8001";
        private static WebChannel instance = null;
        private TcpClient client;

        private WebChannel()
        {
            client = new TcpClient();
            Connect();
        }


        public static WebChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebChannel();
                }
                return instance;
            }
        }

        public bool IsConnected
        {
            get
            {
                return client.Connected;
            }
        }

        // Connects to ImageService server.
        public void Connect()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
                client.Connect(ep);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Reads commands from server (one line at the time).
        // Finishes only on ExitCommand or connection failure.
        public CommandMessage Read()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                string output = reader.ReadLine();
                CommandMessage msg = JsonConvert.DeserializeObject<CommandMessage>(output);
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        // Writes a command message to server.
        public void Write(CommandMessage msg)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                string input = JsonConvert.SerializeObject(msg);
                writer.WriteLine(input);
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
