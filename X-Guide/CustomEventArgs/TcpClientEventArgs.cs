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
        public TcpClient Client { get;}

        public TcpClientEventArgs(TcpClient client)
        {
            Client = client;
        }
    }
}
