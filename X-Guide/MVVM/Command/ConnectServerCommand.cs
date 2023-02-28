using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.Command
{
    internal class ConnectServerCommand : CommandBase
    {
       
        public ConnectServerCommand(SettingViewModel svm)
        {
            Svm = svm;
        }

        private SettingViewModel Svm;

        public override void Execute(object parameter)
        {

            if (Svm.CanEdit) Svm.CanEdit = false;
            else
            {
                Svm.CanEdit = true;
            }
                
           /* if (!_serverService.getServerStatus())
            {
                _serverService.StartServer();
            }
            else
            {
                _serverService.StopServer();
            }*/
            
            
        }
    }

}
