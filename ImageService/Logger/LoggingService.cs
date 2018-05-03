using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageService.Logging
{
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