using System;

namespace ImageService.Logging.Model
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public string Message { get; set; }
        public MessageTypeEnum Status { get; set; }

        public MessageRecievedEventArgs(string msg, MessageTypeEnum st)
        {
            Message = msg;
            Status = st;
        }
    }
}
