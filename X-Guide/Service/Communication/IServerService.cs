using System;
using System.Collections.Generic;
using System.Linq;
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

        bool getServerStatus();
        Task StartServer();
        void StopServer();
    }
}
