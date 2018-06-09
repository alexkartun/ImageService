using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;
using ImageWebService.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageWebService.Models
{
    public class LogsModel
    {
        public List<Log> ServiceLogs { get; set; }
        /// <summary>
        /// Set logs via server request.
        /// </summary>
        /// <param name="channel">Web channel for request.</param>
        public void SetLogs(WebChannel channel)
        {
            ServiceLogs = new List<Log>();
            if (channel.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.LogCommand);
                channel.Write(req);
                CommandMessage answer = channel.Read();
                // Iterate over every two arguments. First for message and second for type.
                for (int i = 0; i < answer.Args.Length; i += 2)
                {
                    string m = answer.Args[i];
                    string t = MessageRecievedEventArgs.GetTypeEnum(Int32.Parse(answer.Args[i + 1])).ToString();
                    Log log = new Log(t, m);
                    // Adds a single log to an observables logs list.
                    ServiceLogs.Add(log);
                }
            }
        }

        /// <summary>
        /// Filter all logs by type.
        /// </summary>
        /// <param name="type">Type filter with.</param>
        /// <param name="channel">Web channel for requesting data.</param>
        public void FilterLogsByType(string type, WebChannel channel)
        {
            SetLogs(channel);
            // Type empty ignore.
            if (type != "")
            {
                List<Log> temp_logs = new List<Log>();
                foreach (Log m in ServiceLogs)
                {
                    if (m.Type.CompareTo(type) == 0)
                    {
                        temp_logs.Add(m);
                    }
                }
                ServiceLogs = temp_logs;
            }
        }
    }
}