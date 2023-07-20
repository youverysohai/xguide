/*using VM.Core;*/

using Autofac;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ManipulatorTcp;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows;
using X_Guide.Enums;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class Step5ViewModel : ViewModelBase
    {
        public object Step5CalibrationModule { get; set; }
        public JogControllerViewModel JogController { get; set; }

        public JogTrackingViewModel JogTracking { get; set; }

        public Step5ViewModel(CalibrationViewModel calibration, ILifetimeScope lifeTimeScope, JogControllerViewModel controller, JogTrackingViewModel tracker)
        {
            tracker.JogTrackingEnabled = !calibration.CalibrationMode;
            JogTracking = tracker; 
            
            switch (calibration.Orientation)
            {
                case Orientation.LookDownward: Step5CalibrationModule = lifeTimeScope.Resolve<Step5LookDownwardConfig>(); break;
                case Orientation.EyeOnHand: Step5CalibrationModule = lifeTimeScope.Resolve<Step5EyeOnHandConfig>(); break;
                case Orientation.MountedOnJoint2: Step5CalibrationModule = lifeTimeScope.Resolve<Step5MountedOnJoint2Config>(); break;
                case Orientation.LookUpward: Step5CalibrationModule = lifeTimeScope.Resolve<Step5LookUpwardConfig>(); break;
            }
            controller.Calibration = calibration;
            JogController = controller;
        }


    }
}