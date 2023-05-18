using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X_Guide.Service.Communication
{
    public class TcpClientInfo //Contains client specific info
    {
        public TcpClient TcpClient { get; set; }

        //
        private ManualResetEventSlim _jogDoneReplyRecieved = new ManualResetEventSlim(false);
        public ManualResetEventSlim JogDoneReplyRecieved => _jogDoneReplyRecieved;
        public bool Jogging { get; set; }

        public TcpClientInfo(TcpClient tcpClient)
        {
            Jogging = false;
            TcpClient = tcpClient;
        }

    }
}
