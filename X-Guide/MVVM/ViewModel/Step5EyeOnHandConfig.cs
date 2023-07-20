using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{

    public class Step5EyeOnHandConfig:ViewModelBase
    {
        private readonly CalibrationViewModel _calibration;
        private readonly IMessenger _messenger;

        public JogControllerViewModel JogController { get; }

        public Step5EyeOnHandConfig(JogControllerViewModel controller,  IMessenger messenger, CalibrationViewModel calibration)
        {
            controller.Calibration = calibration;
            
            _calibration = calibration;
           
            _messenger = messenger;
            JogController = controller;   
        }

    }
}
