using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infastructure.Model
{
    public class Config
    {
        public string Output_Directory { get; }
        public string Source_Name { get; }
        public string Log_Name { get; }
        public string Thumbnail_Size { get; }
        public List<string> Directory_Paths { get; set; }

        public Config(string a_output_directory, string a_source_name, string a_log_name, string a_thumbnail_size)
        {
            Output_Directory = a_output_directory;
            Source_Name = a_source_name;
            Log_Name = a_log_name;
            Thumbnail_Size = a_thumbnail_size;
            Directory_Paths = new List<string>();
        }
    }
}
