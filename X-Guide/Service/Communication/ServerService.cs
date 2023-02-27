using System;
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


namespace X_Guide.Communication.Service
{
    public class ServerService : IServerService
    {
        private int _port { get; }
        private TcpListener _server;
        private int _activeClients = 0;
        public int ActiveClients => _activeClients;
        private bool started = false;

        public event EventHandler<TcpClientEventArgs> ClientEvent;
        public event EventHandler<TcpListenerEventArgs> ListenerEvent;

        CancellationTokenSource cts;


        public ServerService(int port)
        {
            _port = port;

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
            _server = new TcpListener(IPAddress.Any, _port);

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


                    // Handle the client connection in a separate task.
#pragma warning disable CS4014 // This warning has to be suppressed to disallow the await keyword from blocking the task
                    Task.Run(() => HandleClientConnection(client, cts.Token));
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

        private async Task HandleClientConnection(TcpClient client, CancellationToken ct)
        {

            ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
            // Get the stream for reading and writing data.
            NetworkStream stream = client.GetStream();
            ct.Register(() => stream.Close());
            // Buffer for storing incoming data.
            byte[] buffer = new byte[1024];

            string data = "";
            // Enter the data reading loop.
            try
            {

                Interlocked.Increment(ref _activeClients);
                while (!cts.IsCancellationRequested)
                {
                    // Wait for data to be available
                    await Task.Delay(100, ct);

                    // Read incoming data.

                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    data += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (data.EndsWith("\n"))
                    {
                        string[] messages = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string message in messages)
                        {
                            // Handle the received message
                            // Convert the incoming data to a string and display it.
                            Debug.WriteLine("Received message: {0}", message);
                            /*     byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
                                 await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);*/
                        }
                        data = "";
                    }

                    if (bytesRead == 0)
                    {

                        Debug.WriteLine("Client disconnected.");
                        break;
                    }


                }
                client.Close();
                ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
                client.Dispose();
            }

            //throws object dispose exception here
            catch
            {
                /*var errorResponse = Encoding.ASCII.GetBytes("The server connection was forcefully closed!");
                client.GetStream().Write(errorResponse, 0, errorResponse.Length);*/
                Debug.WriteLine($"The connection was forcefully closed by the server.");
                Interlocked.Decrement(ref _activeClients);
                ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
                client.Dispose();
            }



        }


    }

}
