using ImageService.Communication;
using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageServiceGUI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    public class GuiChannel
    {
        private static GuiChannel instance = null;
        private TcpClientChannel tcp_channel;
        private volatile Boolean stop;

        public CommandConverter Converter { get; set; }
        private GuiChannel()
        {
            tcp_channel = new TcpClientChannel();
            Converter = new CommandConverter();
            stop = false;
        }

        public static GuiChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiChannel();
                }
                return instance;
            }
        }

        public Boolean Connect()
        {
            return tcp_channel.Connect();
        }

        public void Disconnect()
        {
            stop = true;
            SendMessage(new CommandMessage((int) CommandEnum.ExitCommand));
            tcp_channel.Disconnect();
        }

        public void Start()
        {
            new Task(() =>
            {
                try
                {
                    while (!stop)
                    {
                        CommandMessage m = GetMessage();
                        if (!Converter.Convert(m)) // Service stopped.
                        {
                            stop = true;
                            tcp_channel.Disconnect();
                        }
                        Thread.Sleep(250);
                    }
                }
                catch (Exception) { }
            }).Start();
        }

        public void SendMessage(CommandMessage m)
        {
            tcp_channel.SendMessage(m);
        }

        public CommandMessage GetMessage()
        {
            return tcp_channel.GetMessage();
        }
    }
}
