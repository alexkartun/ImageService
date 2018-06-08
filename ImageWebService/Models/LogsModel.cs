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
        public LogsModel()
        {
            SetLogs();
        }

        public WebChannel ClientConnection
        {
            get
            {
                return WebChannel.Instance;
            }
        }

        public List<Log> ServiceLogs { get; set; }
        public void SetLogs()
        {
            ServiceLogs = new List<Log>();
            if (ClientConnection.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.LogCommand);
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();
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

        public void FilterLogsByType(string type)
        {
            SetLogs();
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