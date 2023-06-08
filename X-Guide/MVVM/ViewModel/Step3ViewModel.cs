using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using ToastNotifications;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3ViewModel : ViewModelBase
    {
    

        public IVisionCalibrationStep VisionTemplate { get; set; }

        public Step3ViewModel(IVisionCalibrationStep visionTemplate, CalibrationViewModel config)
        {        
            VisionTemplate = visionTemplate;
            visionTemplate.SetConfig(config);
        }

   
    }
}