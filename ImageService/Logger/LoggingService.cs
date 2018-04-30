using ImageService.Logging.Modal;
using System;

namespace ImageService.Logging
{
	//testing change
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs(message, type);
            MessageRecieved(this, msg);
        }
    }
}