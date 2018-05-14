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
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (true)
                        {
                            string commandLine = reader.ReadLine();
                            CommandMessage cmd = JsonConvert.DeserializeObject<CommandMessage>(commandLine);
                            if (cmd.Command == (int)CommandEnum.ExitCommand)
                            {
                                // Client want to exit.
                                break;
                            }
                            CommandRecieved(this, new CommandRecievedEventArgs(cmd.Command, cmd.Args, client));
                        }
                    }
                }
                // Error writing to client occurred, need to remove him from the list of clients.
                catch (Exception)
                {
                    // Error
                }
                finally
                {
                    client.Close();
                    ExitRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.ExitCommand, null, client));
                }
            }).Start();
        }
    }

}

