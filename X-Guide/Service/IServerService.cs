﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;

namespace X_Guide.Service
{
    public interface IServerService
    {
        event EventHandler<TcpClientEventArgs> ClientConnected;
        Task StartServer();
    }
}
