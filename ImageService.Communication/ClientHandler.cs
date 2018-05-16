using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class ClientHandler
    {
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        public event EventHandler<CommandRecievedEventArgs> ExitRecieved;

        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    while (true)
                    {
                        string commandLine = reader.ReadLine();
                        CommandMessage cmd = JsonConvert.DeserializeObject<CommandMessage>(commandLine);
                        CommandRecieved(this, new CommandRecievedEventArgs(cmd.Command, cmd.Args, client));
                    }

                }
                // Connection error with client occurred.
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ExitRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.ExitCommand, null, client));
                }
            }).Start();       
        }
    }

}

