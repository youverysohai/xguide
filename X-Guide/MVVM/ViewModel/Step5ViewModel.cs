/*using VM.Core;*/

using Autofac;
using System.Runtime.Versioning;
using X_Guide.Enums;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class Step5ViewModel : ViewModelBase
    {
        public object Step5CalibrationModule { get; set; }

        public Step5ViewModel(CalibrationViewModel calibration, ILifetimeScope lifeTimeScope)
        {
            switch (calibration.Orientation)
            {
                case Orientation.LookDownward: Step5CalibrationModule = lifeTimeScope.Resolve<Step5LookDownwardConfig>(); break;
                case Orientation.EyeOnHand: Step5CalibrationModule = lifeTimeScope.Resolve<Step5EyeOnHandConfig>(); break;
                case Orientation.MountedOnJoint2: Step5CalibrationModule = lifeTimeScope.Resolve<Step5MountedOnJoint2Config>(); break;
                case Orientation.LookUpward: Step5CalibrationModule = lifeTimeScope.Resolve<Step5LookUpwardConfig>(); break;
            }
        }
    }
}