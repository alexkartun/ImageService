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
    public class Logs
    {
        public Logs()
        {
            ServiceLogs = new List<MessageRecievedEventArgs>();
        }

        public WebChannel ClientConnection
        {
            get
            {
                return WebChannel.Instance;
            }
        }

        [Required]
        public string Type { get; set; }

        public List<MessageRecievedEventArgs> ServiceLogs { get; set; }
        public void SetLogsByType()
        {
            ServiceLogs = new List<MessageRecievedEventArgs>();
            if (ClientConnection.IsConnected)
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.LogCommand);
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();
                for (int i = 0; i < answer.Args.Length; i += 2)
                {
                    string m = answer.Args[i];
                    MessageTypeEnum t = MessageRecievedEventArgs.GetTypeEnum(Int32.Parse(answer.Args[i + 1]));
                    MessageRecievedEventArgs log = new MessageRecievedEventArgs(m, t);
                    // Adds a single log to an observables logs list.
                    if (Type == null || Type == log.Status.ToString())
                        ServiceLogs.Add(log);
                }
            }
        }
    }
}