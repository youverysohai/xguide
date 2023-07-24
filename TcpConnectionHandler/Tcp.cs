using Serilog;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace TcpConnectionHandler
{
    public class Tcp : Attribute
    {
        public event EventHandler<NetworkStreamEventArgs>? DataReceived;

        private readonly ILogger _logger;
        protected readonly TcpConfiguration _configuration;

        public Tcp(TcpConfiguration configuration, ILogger? logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<T?> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                using (ManualResetEventSlim resetEvent = new ManualResetEventSlim())
                {
                    ct.Register(() => { Debug.WriteLine("OperationCanceled"); resetEvent.Set(); });
                    T? data = default(T);

                    EventHandler<NetworkStreamEventArgs> eventHandler = (s, e) =>
                    {
                        try
                        {
                            data = action.Invoke(e);
                            resetEvent.Set();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    };

                    DataReceived += eventHandler;

                    int index = WaitHandle.WaitAny(new WaitHandle[] { ct.WaitHandle, resetEvent.WaitHandle });
                    DataReceived -= eventHandler;

                    return data;
                }
            });
        }

        protected virtual async Task RecieveDataAsync(NetworkStream stream, CancellationToken ct)
        {
            byte[] data = new byte[1024];

            while (!ct.IsCancellationRequested)
            {
                string responseData = string.Empty;
                int bytes = await stream.ReadAsync(data, 0, data.Length);
                if (bytes == 0) break;
                responseData += Encoding.ASCII.GetString(data, 0, bytes);
                _logger.Information($"Received: {responseData}");
                ProcessServerData(responseData, ',', stream);
                await Task.Delay(1000);
            }
            stream.Close();
        }

        protected async Task WriteDataAsync(string data, NetworkStream stream)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data + _configuration.Terminator);
            _ = stream ?? throw new Exception("Can't send data");
            await stream.WriteAsync(bytes, 0, bytes.Length);
            _logger.Information($"Write: {data}");
        }

        private void ProcessServerData(string data, char seperator, NetworkStream stream)
        {
            try
            {
                data = data.Replace("\r", "").Replace("\n", "");
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
            DataReceived?.Invoke(s, e);
        }
    }
}