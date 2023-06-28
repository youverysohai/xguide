using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TcpConnectionHandler.Server
{
    public interface IServerTcp
    {
        event EventHandler<NetworkStreamEventArgs> _dataReceived;

        TcpClientInfo? GetConnectedClientInfo(TcpClient tcpClient);

        Task WriteDataAsync(string data);

        Task<T?> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken());

        bool Status();

        void Start();

        void Stop();

        void SubscribeOnClientConnectionChange(EventHandler<bool> action);

        void UnsubscribeOnClientConnectionChange(EventHandler<bool> action);

        ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient();
    }
}