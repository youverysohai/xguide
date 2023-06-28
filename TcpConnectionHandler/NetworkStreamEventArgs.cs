using System.Net.Sockets;

namespace TcpConnectionHandler
{
    public class NetworkStreamEventArgs : EventArgs
    {
        public NetworkStream NetworkStream { get; }
        public string[]? Data { get; set; }

        public NetworkStreamEventArgs(NetworkStream networkStream, string[]? data = null)
        {
            NetworkStream = networkStream;
            Data = data;
        }
    }
}