﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;

namespace X_Guide.Communication.Service
{
    public interface IServerService
    {

        event EventHandler<NetworkStreamEventArgs> _dataReceived;

        event EventHandler<bool> ClientConnectionChange;
        TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient);
        Task ServerWriteDataAsync(string data);
        Task<T> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken());
        bool getServerStatus();
        Task StartServer();
        void StopServer();
        Task<bool> SendJogCommand(JogCommand jogCommand);
        void SetServerReadTerminator(string terminator);
        ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient();
    }
}
