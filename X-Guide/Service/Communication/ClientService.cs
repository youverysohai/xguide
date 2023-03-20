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

        public ClientService(IPAddress ipAddress, int port)
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

        public async Task<Point> GetVisCenter()
        {
            string flowName = "FindVisCenter";
            await WriteDataAsync($"XGUIDE,{flowName}", _stream);
            Point point = await Task.Run(() => RegisterRequestEventHandler(GetVisCenterEvent));
            Debug.WriteLine(point);
            return point;
            
        }

        private Point GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            string[] data = e.Data;

            if (data.Length == 2)
            {
                Point point = new Point
                {
                    X = double.Parse(data[0]),
                    Y = double.Parse(data[1]),
                };
                return point;

            }
            return null;
        }


        public async Task WriteDataAsync(string data)
        {
            await WriteDataAsync(data, _stream);
        }

    

       
    }
}
