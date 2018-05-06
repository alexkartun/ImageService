using ImageService.Logging.Model;
using System;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        // Event handler for updating the event logger of the service.
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Write the message to event logger via event MessageRecieved.
        /// </summary>
        /// <param name="message"> Message to be written in event entry. </param>
        /// <param name="type"> Type of the message. </param>
        /// <returns> Returns log. </returns>
        void Log(string message, MessageTypeEnum type);
    }
}
