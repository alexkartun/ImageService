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

namespace ImageServiceGUI.Communication
{
    public class GuiChannel
    {
        private static string ip = "127.0.0.1";
        private static string port = "8001";
        private static GuiChannel instance = null;
        private TcpClient client;

        public event EventHandler<CommandRecievedEventArgs> DataRecieved;


        private GuiChannel()
        {
            client = new TcpClient();
            isConnected = Connect();
        }

        private bool isConnected;

        public bool IsConnected => isConnected;
        public void SetIsConnected(bool value) => isConnected = value;

        public static GuiChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiChannel();
                }
                return instance;
            }
        }

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
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                client.Close();
                isConnected = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Read()
        {
            Task t = new Task(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    while (isConnected)
                    {
                        string output = reader.ReadLine();
                        CommandMessage msg = JsonConvert.DeserializeObject<CommandMessage>(output);
                        if (msg.Command == (int)CommandEnum.ExitCommand)
                        {
                            Disconnect();
                            break;
                        }
                        DataRecieved?.Invoke(this, new CommandRecievedEventArgs(msg.Command, msg.Args));
                        Thread.Sleep(250);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
            t.Start();
        }

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
