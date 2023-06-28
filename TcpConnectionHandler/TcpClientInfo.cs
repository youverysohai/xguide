using System.Net.Sockets;

namespace TcpConnectionHandler
{
    public class TcpClientInfo //Contains client specific info
    {
        public TcpClient TcpClient { get; set; }

        private readonly ManualResetEventSlim _jogDoneReplyRecieved = new ManualResetEventSlim(false);

        public ManualResetEventSlim JogDoneReplyRecieved => _jogDoneReplyRecieved;
        public bool Jogging { get; set; }

        public TcpClientInfo(TcpClient tcpClient)
        {
            Jogging = false;
            TcpClient = tcpClient;
        }
    }
}