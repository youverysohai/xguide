using CommunityToolkit.Mvvm.Messaging;
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
        private readonly IMessenger _messenger = new WeakReferenceMessenger();

        public ClientTcp(TcpConfiguration configuration) : base(configuration)
        {
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

                _messenger.Send(new ClientTcpStatusUpdate(true));

                cts = new CancellationTokenSource();
                await RecieveDataAsync(_stream, cts.Token);

                _messenger.Send(new ClientTcpStatusUpdate(false));
            }
            catch (Exception ex)
            {
                _messenger.Send(new ClientTcpStatusUpdate(false));
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