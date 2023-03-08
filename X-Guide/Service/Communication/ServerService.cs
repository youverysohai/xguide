using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communation;
using X_Guide.Service.Communication;


namespace X_Guide.Communication.Service
{
    public class ServerService : IServerService
    {
 
        private int _port { get; }
        private IPAddress _ip { get; }
        private TcpListener _server;

        private bool started = false;

        public event EventHandler<TcpClientEventArgs> ClientEvent;
        public event EventHandler<TcpListenerEventArgs> ListenerEvent;

        public event EventHandler<TcpClientEventArgs> MessageEvent;

        private List<string> pendingCommand = new List<string>();

        private readonly ConcurrentDictionary<int, TcpClientInfo> _connectedClient = new ConcurrentDictionary<int, TcpClientInfo>();

        CancellationTokenSource cts;


        public ServerService(IPAddress ip, int port)
        {
            _port = port;
            _ip = ip;
        }

        public bool getServerStatus()
        {
            return started;
        }

        public void StopServer()
        {
            cts.Cancel();
        }


        public async Task StartServer()
        {
            cts = new CancellationTokenSource();
            // Specify the port number to listen on.

            // Create a TcpListener object.
            _server = new TcpListener(_ip, _port);

            // Start listening for incoming connections.
            try
            {

                _server.Start();
                started = true;
                ListenerEvent?.Invoke(this, new TcpListenerEventArgs(_server));

                //sct = Server Cancellation Token
                CancellationToken sct = cts.Token;
                sct.Register(() =>
                {
                    _server.Stop();
                });




                // Enter the listening loop.
                while (!cts.IsCancellationRequested)
                {

                    // Wait for a client to connect, implemented 
                    TcpClient client = await Task.Run(() => _server.AcceptTcpClientAsync(), sct);
                    Debug.WriteLine("Client connected.");

                    _connectedClient.TryAdd(client.GetHashCode(), new TcpClientInfo(client));
                    ClientEvent?.Invoke(this, new TcpClientEventArgs(client));

                    // Handle the client connection in a separate task.
#pragma warning disable CS4014 // This warning has to be suppressed to disallow the await keyword from blocking the task
                    Task.Run(() => ReadClientCommand(client, cts.Token));
#pragma warning restore CS4014
                }
            }
            //throws socket error here
            catch
            {
                Debug.WriteLine($"Server is closed.");
                started = false;
                ListenerEvent?.Invoke(this, new TcpListenerEventArgs(_server));
            }

        }


        private async Task ReadClientCommand(TcpClient client, CancellationToken ct)
        {



            // Get the stream for reading and writing data.
            NetworkStream stream = client.GetStream();
            _connectedClient.TryAdd(client.GetHashCode(), new TcpClientInfo(client));
            SendMessageAsync("Connected to the server. Please enjoy your stay!\n", stream);

            ct.Register(() => stream.Close());


            Queue<TcpClientEventArgs> commandList = new Queue<TcpClientEventArgs>();

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(100, ct);
                    bool run = true;
                    while (run)
                    {
                        run = await ReadAsync(commandList, ct, client, '\n');
                        RunCommand(commandList);

                    }






                }
                /*   DisposeClient(client);*/
            }

            //throws object dispose exception here
            catch
            {/*
                var errorResponse = Encoding.ASCII.GetBytes("The server connection was forcefully closed!");
                client.GetStream().Write(errorResponse, 0, errorResponse.Length);*/
                Debug.WriteLine($"The connection was forcefully closed by the server.");
                DisposeClient(client);
            }



        }

        private async void RunCommand(Queue<TcpClientEventArgs> commandList)
        {
            while (commandList.Count > 0)
            {
                await Task.Run(() => MessageEvent?.Invoke(this, commandList.Dequeue()));
            }
        }

        public async Task<bool> ReadAsync(Queue<TcpClientEventArgs> commandList, CancellationToken ct, TcpClient client, char delimiter)
        {
            byte[] buffer = new byte[1024];
            string data = "";
            NetworkStream stream = client.GetStream();
            while (!data.EndsWith(delimiter.ToString()))
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                data += Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (bytesRead == 0)
                {
                    Debug.WriteLine("Client disconnected.");
                    return false;
                }

            }

            string[] messages = data.Split(new[] { delimiter.ToString() }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string message in messages)
            {
                Debug.WriteLine(message);
                commandList.Enqueue(new TcpClientEventArgs(client, message));

            }
            return true;
        }



        public void DisposeClient(TcpClient client)
        {
            client.Close();
            _connectedClient.TryRemove(client.GetHashCode(), out _);
            ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
            client.Dispose();
        }

        public async void SendMessageAsync(string message, NetworkStream networkStream)
        {
            var bytes = Encoding.ASCII.GetBytes(message);
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient()
        {
            return _connectedClient;
        }

        public TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient)
        {
            TcpClientInfo tcpClientInfo;
            _connectedClient.TryGetValue(tcpClient.GetHashCode(), out tcpClientInfo);
            return tcpClientInfo;

        }
    }

}
