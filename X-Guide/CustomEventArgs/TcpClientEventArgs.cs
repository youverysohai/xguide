using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.CustomEventArgs
{
    public class TcpClientEventArgs : EventArgs
    {
        public TcpClient TcpClient { get;}
        public string Message { get; set; }

        public TcpClientEventArgs(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
            Message = string.Empty;
        }

        public TcpClientEventArgs(TcpClient tcpClient, string message)
        {
            TcpClient = tcpClient;
            Message = message;
        }
    }
}
