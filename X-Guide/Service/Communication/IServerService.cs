using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;

namespace X_Guide.Communication.Service
{
    public interface IServerService
    {
        event EventHandler<NetworkStreamEventArgs> _dataReceived;

        TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient);

        Task ServerWriteDataAsync(string data);

        Task<T> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken());

        bool Status();

        Task Start();

        void Stop();

        void SetServerReadTerminator(string terminator);

        void SubscribeOnClientConnectionChange(EventHandler<bool> action);

        void UnsubscribeOnClientConnectionChange(EventHandler<bool> action);

        ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient();
    }
}