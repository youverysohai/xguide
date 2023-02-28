using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Guide.Communication.Service
{
    public class ClientService : IClientService
    {
   
        private readonly int _port;
        private TcpClient _client;
        private NetworkStream _stream;
        private IPAddress _ipAddress;

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
                string message = "Hello Server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
                await RecieveDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }

        private async Task RecieveDataAsync()
        {
            byte[] data = new byte[1024];
            string responseData = string.Empty;
            while (_client.Connected)
            {
                int bytes = await _stream.ReadAsync(data, 0, data.Length);
                responseData += Encoding.ASCII.GetString(data, 0, bytes);

                while (_stream.DataAvailable)
                {
                    responseData += Encoding.ASCII.GetString(data, 0, bytes);

                }

                MessageBox.Show(responseData);
                responseData = string.Empty;
            }
            _stream.Close();
            _client.Close();
        }
    }
}
