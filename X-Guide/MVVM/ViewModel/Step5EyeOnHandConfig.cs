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

        public JogTrackingViewModel JogTracking { get; }

        public Step5EyeOnHandConfig(JogTrackingViewModel jogTracking,  IMessenger messenger, CalibrationViewModel calibration)
        {
            
            
            _calibration = calibration;
           
            _messenger = messenger;
            JogTracking = jogTracking;
        }

    }
}
