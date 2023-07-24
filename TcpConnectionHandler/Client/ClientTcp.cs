using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using System.Diagnostics;
using System.Net.Sockets;

namespace TcpConnectionHandler.Client
{
    public class ClientTcp : Tcp, IClientTcp
    {
        private TcpClient _client = new TcpClient();
        private NetworkStream? _stream;
        private CancellationTokenSource? cts;
        public string Flowname = "";
        private readonly IMessenger _messenger;

        public ClientTcp(TcpConfiguration configuration, IMessenger messenger, ILogger? logger) : base(configuration, logger)
        {
            _messenger = messenger;
        }

        public bool IsConnected => _stream != null;

        public async void ConnectServer()
        {
            try
            {
                _client.Close();
                _client.Dispose();
                _client = new TcpClient();
                _client.Connect(_configuration!.IPAddress, _configuration.Port);
                _stream = _client.GetStream();

                _messenger.Send(new ClientStatusChanged(true));

                cts = new CancellationTokenSource();
                await RecieveDataAsync(_stream, cts.Token);
                _messenger.Send(new ClientStatusChanged(false));
            }
            catch (Exception ex)
            {
                _messenger.Send(new ClientStatusChanged(false));
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public async Task WriteDataAsync(string data)
        {
            _ = _stream ?? throw new Exception("Unreachable client.");
            await WriteDataAsync(data, _stream);
        }
    }
}