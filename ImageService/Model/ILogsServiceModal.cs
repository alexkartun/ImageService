using ImageService.Infastructure.Model;
using ImageService.Logging.Model;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ILogsServiceModal
    {
        List<Log> ServiceLogs { get; set; }
        string GetAllLog(out MessageTypeEnum result, TcpClient client = null);
    }
}
