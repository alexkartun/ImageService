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
            StreamWriter writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
            CommandRecievedEventArgs cmd = new CommandRecievedEventArgs(3);
            string output = JsonConvert.SerializeObject(cmd);
            writer.WriteLine(output);
            Console.WriteLine(output);
            Console.WriteLine("{0}", reader.ReadLine());
            cmd.CommandID = 2;
            output = JsonConvert.SerializeObject(cmd);
            Console.WriteLine(output);
            writer.WriteLine(output);
            Console.WriteLine("{0}", reader.ReadLine());
        }
    }
}
