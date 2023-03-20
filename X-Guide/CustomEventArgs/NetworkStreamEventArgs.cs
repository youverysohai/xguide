using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.CustomEventArgs
{
    public class NetworkStreamEventArgs : EventArgs
    {
        public NetworkStream NetworkStream { get; }
        public string[] Data { get; set; }

        public NetworkStreamEventArgs(NetworkStream networkStream, string[] data = null)
        {
            NetworkStream = networkStream;
            Data = data;
        }
  
    }
}
