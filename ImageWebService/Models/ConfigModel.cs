using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageWebService.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    public class ConfigModel
    {
        public ConfigModel()
        {
            SetConfigData();
        }

        public WebChannel ClientConnection
        {
            get
            {
                return WebChannel.Instance;
            }
        }

        public void SetConfigData()
        {
            DirectoryHandlers = new List<string>();
            OutputDir = SourceName = LogName = ThumbnailSize = "";
            if (ClientConnection.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.GetConfigCommand);
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();
                OutputDir = answer.Args[0];
                SourceName = answer.Args[1];
                LogName = answer.Args[2];
                ThumbnailSize = answer.Args[3];
                for (int i = 5; i < answer.Args.Length; i++)
                {
                    DirectoryHandlers.Add(answer.Args[i]);
                }
            }
        }

        public void RemoveHandler(string handler)
        {
            if (ClientConnection.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.CloseCommand, new string[] { handler });
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();
                DirectoryHandlers.Remove(handler);
            }
        }

        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<String> DirectoryHandlers { get; set; }
    }
}