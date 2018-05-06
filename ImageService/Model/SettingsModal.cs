using ImageService.Infastructure.Model;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace ImageService.Model
{
    public class SettingsModal : ISettingsModal
    {
        public Config ServiceConfig { get; set; }

        public SettingsModal(string src_name, string log_name, string output_dir_path, string thumbnail_size)
        {
            ServiceConfig = new Config(src_name, log_name, output_dir_path, thumbnail_size);
        }

        public string GetConfig(out MessageTypeEnum result, TcpClient client)
        {
            result = MessageTypeEnum.INFO;
            string output = JsonConvert.SerializeObject(ServiceConfig);
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(output);
            }
            return output;
        }
    }
}
