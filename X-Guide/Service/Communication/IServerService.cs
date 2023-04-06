using System;
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
        event EventHandler<TcpClientEventArgs> ClientEvent;
        event EventHandler<TcpListenerEventArgs> ListenerEvent;
        event EventHandler<TcpClientEventArgs> MessageEvent;

        TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient);
        Task ServerWriteDataAsync(string data);

        bool getServerStatus();
        Task StartServer();
        void StopServer();
        Task<bool> SendJogCommand(JogCommand jogCommand);
        void SetServerReadTerminator(string terminator);
        ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient();
    }
}
