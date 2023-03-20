using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using X_Guide.Communication.Service;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {

        private CalibrationViewModel _setting;
        private readonly IServerService _serverService;

        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set { _setting = value;
                OnPropertyChanged();
            }
        }

      

        
        public override ViewModelBase GetNextViewModel()
        {
            
            return new Step5ViewModel(_setting, _serverService);
        }
        
    
        public Step4ViewModel(CalibrationViewModel setting, IServerService serverService)
        {
            _setting = setting;
            _serverService = serverService;
        }


    }
}
