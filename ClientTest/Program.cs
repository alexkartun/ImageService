using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Int32.Parse("8000"));
            bool running = true;
            TcpClient client = new TcpClient();
            client.Connect(ep);
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                new Task(() =>
                {
                    while(true)
                    {
                        string output = reader.ReadLine();
                        CommandMessage m = JsonConvert.DeserializeObject<CommandMessage>(output);
                        foreach (string st in m.Args)
                        {
                            Console.WriteLine(st);
                        }
                    }
                }).Start();

                CommandMessage msg1 = new CommandMessage((int)CommandEnum.GetConfigCommand);
                string input = JsonConvert.SerializeObject(msg1);
                writer.WriteLine(input);


                CommandMessage msg2 = new CommandMessage((int)CommandEnum.LogCommand);
                input = JsonConvert.SerializeObject(msg2);
                writer.WriteLine(input);

                string[] asd = { @"C:\Users\Kartun\Downloads" };

                CommandMessage msg3 = new CommandMessage((int)CommandEnum.CloseCommand, asd);
                input = JsonConvert.SerializeObject(msg3);
                writer.WriteLine(input);

                CommandMessage msg4 = new CommandMessage((int)CommandEnum.GetConfigCommand);
                input = JsonConvert.SerializeObject(msg1);
                writer.WriteLine(input);

                CommandMessage msg5 = new CommandMessage((int)CommandEnum.ExitCommand);
                input = JsonConvert.SerializeObject(msg1);
                writer.WriteLine(input);
                while (true) ;
     
            }
            client.Close();
        }
    }
}
