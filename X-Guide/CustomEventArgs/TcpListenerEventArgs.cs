using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.CustomEventArgs
{
    public class TcpListenerEventArgs : EventArgs
    {
        public TcpListener TcpListener { get; }

        public TcpListenerEventArgs(TcpListener tcpListener)
        {
            TcpListener = tcpListener;
        }
    }
}
