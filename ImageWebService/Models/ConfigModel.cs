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
        /// <summary>
        /// Set config data via server request.
        /// </summary>
        /// <param name="channel">Web channel for requests.</param>
        public void SetConfigData(WebChannel channel)
        {
            DirectoryHandlers = new List<string>();
            OutputDir = SourceName = LogName = ThumbnailSize = "";
            if (channel.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.GetConfigCommand);
                channel.Write(req);
                CommandMessage answer = channel.Read();
                OutputDir = answer.Args[0];
                SourceName = answer.Args[1];
                LogName = answer.Args[2];
                ThumbnailSize = answer.Args[3];
                for (int i = 4; i < answer.Args.Length; i++)
                {
                    DirectoryHandlers.Add(answer.Args[i]);
                }
            }
        }

        /// <summary>
        /// Remove handler on server side.
        /// </summary>
        /// <param name="handler">Handler to remove.</param>
        /// <param name="channel">Web channel for requests.</param>
        public void RemoveHandler(string handler, WebChannel channel)
        {
            if (handler != null && handler != "")
            {
                if (channel.IsConnected())
                {
                    CommandMessage req = new CommandMessage((int)CommandEnum.CloseCommand, new string[] { handler });
                    channel.Write(req);
                    CommandMessage answer = channel.Read();
                    DirectoryHandlers.Remove(handler);
                }
            }
        }

        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<String> DirectoryHandlers { get; set; }
    }
}