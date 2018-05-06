using ImageService.Infastructure.Model;
using ImageService.Logging.Model;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ISettingsModal
    {
        Config ServiceConfig { get; set; }
        string GetConfig(out MessageTypeEnum result, TcpClient client);
    }
}
