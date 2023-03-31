using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
    
        private CalibrationViewModel _setting;
        private readonly IServerService _serverService;
        private readonly IClientService _clientService;

        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
                OnPropertyChanged();
            }
        }


        public Step2ViewModel(CalibrationViewModel setting, IServerService serverService, IClientService clientService)
        {
            Setting = setting;
            _serverService = serverService;
            _clientService = clientService;
            _setting.Orientation = 1; 

        }


    }
}
