using ImageService.Communication.Model;
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
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.AutoFlush = true;
                    while (true)
                    {
                        string commandLine = reader.ReadLine();
                        CommandMessage cmd = JsonConvert.DeserializeObject<CommandMessage>(commandLine);
                        string output = image_controller.ExecuteCommand(cmd.Command, cmd.Args,
                            out MessageTypeEnum status);
                        writer.WriteLine(output);
                        // logging_service.Log(output, status);
                        // Add new log to logs modal.
                        // image_controller.LogsModal.ServiceLogs.Add(new Log(output, status));
                    }
                }
                //client.Close();
                //clients.Remove(client);
            }).Start();
        }
    }
}
