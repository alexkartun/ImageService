using ImageService.Logging.Model;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ISettingsModal
    {
        List<string> Directory_Paths { get; set; }
        string GetSettings(out MessageTypeEnum result, TcpClient client);
    }
}
