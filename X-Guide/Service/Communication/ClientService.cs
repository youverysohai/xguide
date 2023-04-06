using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;
using X_Guide.Service;
using X_Guide.Service.Communication;
using Xlent_Vision_Guided;

namespace X_Guide.Communication.Service
{
    public class ClientService : TCPBase, IClientService
    {

        private readonly int _port;
        private TcpClient _client;
        private NetworkStream _stream;
        private IPAddress _ipAddress;
        private CancellationTokenSource cts;
        public string Flowname = "";
        private BackgroundService _connectServer;

        public ClientService(IPAddress ipAddress, int port, string terminator = null) : base(terminator)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public async Task ConnectServer()
        {
            
            try
            {
                _client = new TcpClient();
                _client.Connect(_ipAddress, _port);
                _stream = _client.GetStream();
                cts = new CancellationTokenSource();
                await RecieveDataAsync(_stream, cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }

      


        public async Task WriteDataAsync(string data)
        {
            await WriteDataAsync(data, _stream);
        }

       
    }
}
