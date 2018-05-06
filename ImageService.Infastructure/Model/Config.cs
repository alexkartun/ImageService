using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infastructure.Model
{
    public class Config
    {
        public string Output_Directory { get; set; }
        public string Source_Name { get; set; }
        public string Log_Name { get; set; }
        public string Thumbnail_Size { get; set; }
        public List<string> Directory_Paths { get; set; }

        public Config(string output_directory, string source_name, string log_name, string thumbnail_size,
            List<string> directory_paths)
        {
            Output_Directory = output_directory;
            Source_Name = source_name;
            Log_Name = log_name;
            Thumbnail_Size = thumbnail_size;
            Directory_Paths = directory_paths;
        }
    }
}
