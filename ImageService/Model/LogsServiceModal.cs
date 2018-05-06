using ImageService.Infastructure.Modal;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class LogsServiceModal : ILogsServiceModal
    {

        public List<Log> ServiceLogs { get; set; }

        public LogsServiceModal(List<Log> logs)
        {
            ServiceLogs = logs;
        }

        public string GetAllLog(out MessageTypeEnum result)
        {
            result = MessageTypeEnum.INFO;
            string output = JsonConvert.SerializeObject(ServiceLogs);         
            return output;
        }

    }
}
