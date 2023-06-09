using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.Logging;
using X_Guide.MessageToken;
using X_Guide.MVVM.Model;
using X_Guide.Service.Communication;

namespace X_Guide.Communication.Service
{
    public class ClientService : TCPBase, IClientService, ILoggable
    {
        private readonly int _port;
        private IMessenger _messenger;
        private TcpClient _client = new TcpClient();
        private NetworkStream _stream;
        private readonly IPAddress _ipAddress;
        private CancellationTokenSource cts;
        public string Flowname = "";

        public ClientService(IPAddress ipAddress, int port, IMessenger messenger, string terminator = null) : base(terminator)
        {
            _ipAddress = ipAddress;
            _port = port;
            _messenger = messenger;
        }

        public bool IsConnected => _stream != null;

        public async Task ConnectServer()
        {
            try
            {
                _client.Close();
                _client.Dispose();
                _client = new TcpClient();
                _client.Connect(_ipAddress, _port);
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
            _ = _stream ?? throw new Exception(StrRetriver.Get("CL000"));
            await WriteDataAsync(data, _stream);
        }

        public void Log(string message)
        {
            throw new NotImplementedException();
        }
    }
}