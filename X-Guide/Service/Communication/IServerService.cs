using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;

namespace X_Guide.Communication.Service
{
    public interface IServerService
    {
        event EventHandler<TcpClientEventArgs> ClientEvent;
        event EventHandler<TcpListenerEventArgs> ListenerEvent;
        event EventHandler<TcpClientEventArgs> CommandEvent;

        void SendMessageAsync(string message, NetworkStream networkStream);
        bool getServerStatus();
        Task StartServer();
        void StopServer();
    }
}
