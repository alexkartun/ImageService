﻿using ImageService.Infastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    interface IServer
    {
        void Start();
        void Stop();
    }
}
