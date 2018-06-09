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
    public class WebChannel
    {
        private static string ip = "127.0.0.1";
        private static string port = "8001";
        private TcpClient client;

        public WebChannel()
        {
            client = new TcpClient();
            Connect();
        }

        /// <summary>
        /// Check for connection to host.
        /// </summary>
        /// <returns>Connectded = true. Not connected = false.</returns>
        public bool IsConnected()
        {
            return client.Connected;
        }

        /// <summary>
        /// Connect to server/host.
        /// </summary>
        /// <returns>Return true on success and false on failure.</returns>
        public Boolean Connect()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
                client.Connect(ep);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Read sync' data from server.
        /// </summary>
        /// <returns>Return the data accepted.</returns>
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

        /// <summary>
        /// Write data to server sync'.
        /// </summary>
        /// <param name="msg">Data to write.</param>
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
