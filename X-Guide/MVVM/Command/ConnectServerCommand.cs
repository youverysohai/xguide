using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;

namespace X_Guide.MVVM.Command
{
    internal class ConnectServerCommand : CommandBase
    {
        IServerService _serverService;

        public ConnectServerCommand(IServerService serverService)
        {
            _serverService = serverService;
        }


        public override void Execute(object parameter)
        {
            if (!_serverService.getServerStatus())
            {
                _serverService.StartServer();
            }
            else
            {
                _serverService.StopServer();
            }
            
            
        }
    }

}
