using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;
using X_Guide.MessageToken;
using X_Guide.Service.Communication;

namespace X_Guide.Communication.Service
{
    public class ServerService : TCPBase, IServerService
    {
        private int _port { get; }
        private IPAddress _ip { get; }

        private readonly IMessenger _messenger;
        private TcpListener _server;
        private readonly bool _canExecute = false;

        public event EventHandler<TcpClientEventArgs> ClientEvent;

        public event EventHandler<TcpListenerEventArgs> ListenerEvent;

        public event EventHandler<bool> ClientConnectionChange;

        private readonly ConcurrentDictionary<int, TcpClientInfo> _connectedClient = new ConcurrentDictionary<int, TcpClientInfo>();

        private bool _isAnyClientConnected;

        protected bool IsAnyClientConnected
        {
            get { return _isAnyClientConnected; }
            set
            {
                _isAnyClientConnected = value;
                _messenger.Send(new ConnectionStatusChanged(value));
            }
        }

        private CancellationTokenSource cts;

        public ServerService(IPAddress ip, int port, string terminator, IMessenger messenger) : base(terminator)
        {
            _port = port;
            _ip = ip;
            _messenger = messenger;
            _ = Start();
        }

        public bool Status()
        {
            return _canExecute;
        }

        public void Stop()
        {
            cts.Cancel();
        }

        public async Task Start()
        {
            cts = new CancellationTokenSource();
            _server = new TcpListener(_ip, _port);

            try
            {
                _server.Start();

                CancellationToken sct = cts.Token;

                while (!cts.IsCancellationRequested)
                {
                    TcpClient client = await Task.Run(() => _server.AcceptTcpClientAsync(), cts.Token);

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
                ListenerEvent?.Invoke(this, new TcpListenerEventArgs(_server));
            }
        }

        public void DisposeClient(TcpClient client)
        {
            client.Close();
            _connectedClient.TryRemove(client.GetHashCode(), out _);
            ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
            client.Dispose();
        }

        public ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient()
        {
            return _connectedClient;
        }

        public TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient)
        {
            if (tcpClient == null) return null;
            TcpClientInfo tcpClientInfo;
            _connectedClient.TryGetValue(tcpClient.GetHashCode(), out tcpClientInfo);
            return tcpClientInfo;
        }

        public void SetServerReadTerminator(string terminator)
        {
            Terminator = terminator;
        }

        public async Task ServerWriteDataAsync(string data)
        {
            await WriteDataAsync(data, _connectedClient.FirstOrDefault().Value.TcpClient.GetStream());
        }

        public void SubscribeOnClientConnectionChange(EventHandler<bool> action)
        {
            ClientConnectionChange += action;
            //ClientConnectionChange?.Invoke(this, _canExecute);
        }

        public void UnsubscribeOnClientConnectionChange(EventHandler<bool> action)
        {
            ClientConnectionChange -= action;
        }
    }
}