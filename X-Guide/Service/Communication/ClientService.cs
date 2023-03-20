using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.Service;
using Xlent_Vision_Guided;

namespace X_Guide.Communication.Service
{
    public class ClientService : IClientService
    {

        private readonly int _port;
        private TcpClient _client;
        private NetworkStream _stream;
        private IPAddress _ipAddress;
        private Queue<string> _queue;
        private CancellationTokenSource cts;
        private event EventHandler<string[]> _dataReceived;
        
       

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

                await RecieveDataAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public async Task WriteDataAsync(string stringData)
        {
            byte[] data = Encoding.ASCII.GetBytes(stringData);
            await _stream.WriteAsync(data, 0, data.Length);

        }

        public async Task<Point> GetVisCenter()
        {
            string flowName = "FindVisCenter";
            await WriteDataAsync($"XGUIDE,{flowName}");
            Point point = await Task.Run(() => RegisterRequestEventHandler(GetVisCenterEvent));
            Debug.WriteLine(point);
            return point;
            
        }


        private Point GetVisCenterEvent(string[] data)
        {
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



        private T RegisterRequestEventHandler<T>(Func<string[], T> action)
        {
            
            using(ManualResetEventSlim resetEvent = new ManualResetEventSlim())
            {
                T data = default(T);

                EventHandler<string[]> eventHandler = (s, e) =>
                {
                    data = action.Invoke(e);
                    if(data != null)  resetEvent.Set();
                };

                _dataReceived += eventHandler;

                resetEvent.Wait();

                _dataReceived -= eventHandler;

                return data;
            }
        }




        public void ProcessServerData(string data)
        {
            try
            {       
                string[] segment = data.Split(',');
                _dataReceived?.Invoke(this, segment);
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }


        }



        private async Task RecieveDataAsync()
        {

            byte[] data = new byte[1024];
          

            while (_client.Connected)
            {
                string responseData = string.Empty;
                int bytes = await _stream.ReadAsync(data, 0, data.Length);
                responseData += Encoding.ASCII.GetString(data, 0, bytes);
                ProcessServerData(responseData);
                await Task.Delay(1000);
            }

            _stream.Close();
            _client.Close();
        }


    }
}
