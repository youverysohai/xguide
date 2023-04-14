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
    public class TCPBase : Attribute

    {

        public event EventHandler<NetworkStreamEventArgs> _dataReceived;

        private string _terminator;
        protected string Terminator { get => _terminator; set => _terminator = value ?? "\n"; }
        public async Task<T> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                using (ManualResetEventSlim resetEvent = new ManualResetEventSlim())
                {
                    ct.Register(() => { Debug.WriteLine("OperationCanceled"); resetEvent.Set(); });
                    T data = default(T);

                    EventHandler<NetworkStreamEventArgs> eventHandler = (s, e) =>
                    {
                        try
                        {
                            data = action.Invoke(e);
                            resetEvent.Set();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    };

                    _dataReceived += eventHandler;


                    int index = WaitHandle.WaitAny(new WaitHandle[] { ct.WaitHandle, resetEvent.WaitHandle });
                    _dataReceived -= eventHandler;

                    return data;
                }
            });
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
                if (bytes == 0) break;
                responseData += Encoding.ASCII.GetString(data, 0, bytes);
                ProcessServerData(responseData, ',', stream);
                await Task.Delay(1000);

            }
            stream.Close();
        }

        protected async Task WriteDataAsync(string data, NetworkStream stream)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data + Terminator);
             _ = stream ?? throw new Exception(StrRetriver.Get("CL000"));
            await stream.WriteAsync(bytes, 0, bytes.Length);
           
              
        }

        private void ProcessServerData(string data, char seperator, NetworkStream stream)
        {
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
