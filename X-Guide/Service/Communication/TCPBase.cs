using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;

namespace X_Guide.Service.Communication
{
    public class TCPBase
    {

        protected event EventHandler<NetworkStreamEventArgs> _dataReceived;

        private string _terminator;
        protected string Terminator { get => _terminator; set => _terminator = value ?? "\n"; }
        protected T RegisterRequestEventHandler<T>(Func<NetworkStreamEventArgs, T> action)
        {

            using (ManualResetEventSlim resetEvent = new ManualResetEventSlim())
            {
                T data = default(T);

                EventHandler<NetworkStreamEventArgs> eventHandler = (s, e) =>
                {
                    data = action.Invoke(e);
                    if (data != null) resetEvent.Set();
                };

                _dataReceived += eventHandler;

                resetEvent.Wait();

                _dataReceived -= eventHandler;

                return data;
            }
        }

        public TCPBase(string terminator)
        {
            Terminator = terminator;
        }

        virtual protected async Task RecieveDataAsync(NetworkStream stream, CancellationToken ct)
        {

            byte[] data = new byte[1024];


            while (!ct.IsCancellationRequested)
            {
                string responseData = string.Empty;
                int bytes = await stream.ReadAsync(data, 0, data.Length);
                responseData += Encoding.ASCII.GetString(data, 0, bytes);
                ProcessServerData(responseData, ',', stream);
                await Task.Delay(1000);

            }
            stream.Close();
        }

        protected async Task WriteDataAsync(string data, NetworkStream stream)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data + Terminator);
            await stream.WriteAsync(bytes, 0, bytes.Length);

        }

        private void ProcessServerData(string data, char seperator, NetworkStream stream)
        {
            Debug.WriteLine("Recieved data from server!", data);
            try
            {
                string[] segment = data.Split(seperator);
                OnDataRecieved(this, new NetworkStreamEventArgs(stream, segment));
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        protected void OnDataRecieved(object s, NetworkStreamEventArgs e)
        {
            _dataReceived?.Invoke(s, e);
        }
    }
}
