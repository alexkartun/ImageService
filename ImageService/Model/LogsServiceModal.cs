using ImageService.Infastructure.Model;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public class LogsServiceModal : ILogsServiceModal
    {

        public List<Log> ServiceLogs { get; set; }

        public LogsServiceModal()
        {
            ServiceLogs = new List<Log>();
        }

        public string GetAllLog(out MessageTypeEnum result, TcpClient client = null)
        {
            result = MessageTypeEnum.INFO;
            string output = JsonConvert.SerializeObject(ServiceLogs);         
            return output;
        }

    }
}
