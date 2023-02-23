using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.CustomEventArgs;


namespace X_Guide.Service
{
    public class ServerService
    {
        private int _port { get; }
        private bool _connected = false;
        private TcpListener _server;
        public event EventHandler<TcpClientEventArgs> ClientConnected;
        public ServerService(int port)
        {
            _port = port;
        }

        public async Task StartServer()
        {

            // Specify the port number to listen on.


            // Create a TcpListener object.
            _server = new TcpListener(IPAddress.Any, _port);
          
            // Start listening for incoming connections.

            _server.Start();
            
          Debug.WriteLine("Server started, waiting for clients...");

            // Enter the listening loop.
            while (true)
            {
                // Wait for a client to connect.
                TcpClient client = await _server.AcceptTcpClientAsync();
                Debug.WriteLine("Client connected.");
                ClientConnected?.Invoke(this, new TcpClientEventArgs(client));
                
                // Handle the client connection in a separate task.
#pragma warning disable CS4014 // This warning has to be suppressed to disallow the await keyword from blocking the task
                Task.Run(() => HandleClientConnection(client));
#pragma warning restore CS4014 
            }
        }

        private async Task HandleClientConnection(TcpClient client)
        {
            // Get the stream for reading and writing data.
            NetworkStream stream = client.GetStream();

            // Buffer for storing incoming data.
            byte[] buffer = new byte[1024];

            string data = "";
            // Enter the data reading loop.
            while (true)
            {
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
                        byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
                        await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                    }
                    data = "";
                }

                // If no data was read, the connection was closed by the client.
                if (bytesRead == 0)
                {
                    Debug.WriteLine("Client disconnected.");
                    break;
                }

                // Echo the data back to the client.
            }
            _connected = false;
            // Close the client connection.
            client.Close();
        }
    }

}
