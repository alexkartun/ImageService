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
    public class SettingsModal : ISettingsModal
    {
        public Config ServiceConfig { get; set; }

        public SettingsModal(Config cfg)
        {
            ServiceConfig = cfg;
        }

        public string GetConfig(out MessageTypeEnum result)
        {
            result = MessageTypeEnum.INFO;
            string output = JsonConvert.SerializeObject(ServiceConfig);
            return output;
        }
    }
}
