using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;

namespace TcpConnectionHandler.Server
{
    public class ServerTcp : Tcp, IServerTcp
    {
        private TcpListener? _server;
        private readonly bool _canExecute = false;

        public event EventHandler<NetworkStreamEventArgs>? _dataReceived;

        private readonly ConcurrentDictionary<int, TcpClientInfo> _connectedClient = new ConcurrentDictionary<int, TcpClientInfo>();

        private bool _isAnyClientConnected;

        protected bool IsAnyClientConnected
        {
            get { return _isAnyClientConnected; }
            set
            {
                _isAnyClientConnected = value;
            }
        }

        private CancellationTokenSource? cts;

        public ServerTcp(TcpConfiguration configuration) : base(configuration)
        {
        }

        public bool Status()
        {
            return _canExecute;
        }

        public void Stop()
        {
            cts!.Cancel();
        }

        public async void Start()
        {
            cts = new CancellationTokenSource();
            _server = new TcpListener(_configuration.IPAddress, _configuration.Port);

            try
            {
                _server.Start();

                while (!cts.IsCancellationRequested)
                {
                    TcpClient client = await Task.Run(_server.AcceptTcpClientAsync, cts.Token);

                    Debug.WriteLine("Client connected.");

                    _connectedClient.TryAdd(client.GetHashCode(), new TcpClientInfo(client));
                    IsAnyClientConnected = true;
                    // Handle the client connection in a separate task.
#pragma warning disable CS4014 // This warning has to be suppressed to disallow the await keyword from blocking the task

                    Task.Run(async () =>
                    {
                        await RecieveDataAsync(client.GetStream(), cts.Token);
                        _connectedClient.TryRemove(client.GetHashCode(), out _);
                        if (_connectedClient.Count == 0) { IsAnyClientConnected = false; }
                    });
#pragma warning restore CS4014
                }
            }
            catch
            {
                Debug.WriteLine($"Server is closed.");
            }
        }

        public void DisposeClient(TcpClient client)
        {
            client.Close();
            _connectedClient.TryRemove(client.GetHashCode(), out _);
            client.Dispose();
        }

        public ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient()
        {
            return _connectedClient;
        }

        public TcpClientInfo? GetConnectedClientInfo(TcpClient tcpClient)
        {
            if (tcpClient == null) return null;
            TcpClientInfo? tcpClientInfo;
            _connectedClient.TryGetValue(tcpClient.GetHashCode(), out tcpClientInfo);
            return tcpClientInfo;
        }

        public async Task WriteDataAsync(string data)
        {
            if (_connectedClient.Count < 1) throw new Exception("No connected client");
            foreach (var client in _connectedClient)
            {
                await WriteDataAsync(data, client.Value.TcpClient.GetStream());
            }
        }

        public void SubscribeOnClientConnectionChange(EventHandler<bool> action)
        {
            //ClientConnectionChange += action;
            //ClientConnectionChange?.Invoke(this, _canExecute);
        }

        public void UnsubscribeOnClientConnectionChange(EventHandler<bool> action)
        {
            //ClientConnectionChange -= action;
        }
    }
}