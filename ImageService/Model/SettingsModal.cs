using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class SettingsModal : ISettingsModal
    {
        private string output_directory;
        private string source_name;
        private string log_name;
        private string thumbnail_size;
        public List<string> Directory_Paths { get; set; }

        public SettingsModal(string src_name, string log_name, string output_dir_path, string thumbnail_size)
        {
            output_directory = output_dir_path;
            source_name = src_name;
            this.log_name = log_name;
            this.thumbnail_size = thumbnail_size;
            Directory_Paths = new List<string>();
        }

        public string GetSettings(out MessageTypeEnum result, TcpClient client)
        {
            result = MessageTypeEnum.INFO;
			// Static string type array holding APPCONFIG settings.
            string[] service_data = { output_directory, source_name, log_name, thumbnail_size };
			// Dynamic string type array (unfixed number of dirs) holding all active directory handlers. 
			string[] dir_paths = Directory_Paths.ToArray();
			// Union those to strig arrays.
            string[] args = service_data.Union(dir_paths).ToArray();
            CommandMessage msg = new CommandMessage((int) CommandEnum.GetConfigCommand, args);
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
            return "Got command: " + (CommandEnum.GetConfigCommand).ToString() + " Args: ";
        }
    }
}
