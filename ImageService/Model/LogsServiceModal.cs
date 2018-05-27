using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

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
			// Executes log command.
            CommandMessage msg = new CommandMessage((int)CommandEnum.LogCommand, ServiceLogs.ToArray());
			// Converts to string via Json.
			string output = JsonConvert.SerializeObject(msg);
            try
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(output);
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = MessageTypeEnum.FAIL;
            }
            return "Got command: " + (CommandEnum.LogCommand).ToString() + " Args: ";
        }
        

    }
}
