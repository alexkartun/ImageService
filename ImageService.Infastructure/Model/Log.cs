﻿using ImageService.Logging.Model;

namespace ImageService.Infastructure.Model
{
    public class Log
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        public Log(string msg, MessageTypeEnum type)
        {
            Message = msg;
            Status = type;
        }
    }
}