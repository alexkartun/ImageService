using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ImageService.Model
{
    public class LogsServiceModal : ILogsServiceModal
    {

        public List<string> ServiceLogs { get; set; }

        public LogsServiceModal()
        {
            ServiceLogs = new List<string>();
        }

        public string GetAllLog(out MessageTypeEnum result, TcpClient client)
        {
            result = MessageTypeEnum.INFO;
            CommandMessage msg = new CommandMessage((int)CommandEnum.LogCommand, ServiceLogs.ToArray());
            string output = JsonConvert.SerializeObject(msg);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
            writer.WriteLine(output);
            return "Got command ID: " + ((int)CommandEnum.LogCommand).ToString() + " Args: ";
        }
        

    }
}
