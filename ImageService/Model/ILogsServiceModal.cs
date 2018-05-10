using ImageService.Logging.Model;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ILogsServiceModal
    {
        List<string> ServiceLogs { get; set; }
        string GetAllLog(out MessageTypeEnum result, TcpClient client);
    }
}
