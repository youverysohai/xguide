﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlent_Vision_Guided;

namespace X_Guide.Communication.Service
{
    public interface IClientService
    {
        Task ConnectServer();
        Task WriteDataAsync(string stringData);
        Task<Point> GetVisCenter();
    }
}
