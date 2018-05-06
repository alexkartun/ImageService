using ImageService.Infastructure.Modal;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public interface ILogsServiceModal
    {
        List<Log> ServiceLogs { get; set; }
        string GetAllLog(out MessageTypeEnum result);
    }
}
