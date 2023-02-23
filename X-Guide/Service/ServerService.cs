using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Service
{
    public static class ServerService
    {

        public static async Task Main()
        {
            // Specify the port number to listen on.
            int port = 1234;

            // Create a TcpListener object.
            TcpListener server = new TcpListener(IPAddress.Any, 3000);

            // Start listening for incoming connections.
            server.Start();
            Debug.WriteLine("Server started, waiting for clients...");

            // Enter the listening loop.
            while (true)
            {
                // Wait for a client to connect.
                TcpClient client = await server.AcceptTcpClientAsync();
                Debug.WriteLine("Client connected.");

                // Handle the client connection in a separate task.
                await Task.Run(() => HandleClientConnection(client));
            }
        }

        static async Task HandleClientConnection(TcpClient client)
        {
            // Get the stream for reading and writing data.
            NetworkStream stream = client.GetStream();

            // Buffer for storing incoming data.
            byte[] buffer = new byte[1024];

            // Enter the data reading loop.
            while (true)
            {
                // Read incoming data.
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                // If no data was read, the connection was closed by the client.
                if (bytesRead == 0)
                {
                    Debug.WriteLine("Client disconnected.");
                    break;
                }

                // Convert the incoming data to a string and display it.
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Debug.WriteLine("Received: {0}", data);

                // Echo the data back to the client.
                byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
                await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
            }

            // Close the client connection.
            client.Close();
        }
    }

}
