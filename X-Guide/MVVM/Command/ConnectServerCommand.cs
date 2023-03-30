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
        public IClientService _clientService;
        public SettingViewModel _settingViewModel { get; }
        public ConnectServerCommand(SettingViewModel settingViewModel)
        {
           
            _settingViewModel = settingViewModel;
    
        }

       

        public override void Execute(object parameter)
        {
            MessageBox.Show("Start connecting...");
            _clientService = new ClientService(IPAddress.Parse(_settingViewModel.Machine.Ip), int.Parse(_settingViewModel.Machine.Port));
            _clientService.ConnectServer();
            
        }
    }

}
