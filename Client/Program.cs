using ImageService.Infastructure.Modal;
using ImageService.Infastructure.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
                CommandRecievedEventArgs cmd = new CommandRecievedEventArgs(2);
                string output = JsonConvert.SerializeObject(cmd);
                writer.WriteLine(output);
                string input = reader.ReadLine();
                Config cfg = JsonConvert.DeserializeObject<Config>(input);
                Console.WriteLine(cfg.Log_Name);
                Console.WriteLine(cfg.Output_Directory);
                Console.WriteLine(cfg.Source_Name);
                Console.WriteLine(cfg.Thumbnail_Size);
                foreach (string dir in cfg.Directory_Paths)
                {
                    Console.WriteLine(dir);
                }
            
            CommandRecievedEventArgs c = new CommandRecievedEventArgs(3);
            output = JsonConvert.SerializeObject(c);
            writer.WriteLine(output);
            input = reader.ReadLine();


            List<Log> logs = JsonConvert.DeserializeObject<List<Log>>(input);
            foreach (Log log in logs)
            {
                Console.WriteLine(log.Message);
                Console.WriteLine((int)log.Status);
            }
            

        }

    }
}
