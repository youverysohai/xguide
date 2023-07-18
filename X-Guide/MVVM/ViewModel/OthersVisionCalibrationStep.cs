using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class OthersVisionCalibrationStep : ViewModelBase, IVisionCalibrationStep
    {
        public string VisionTriggerFormat { get; set; }
        public OthersVisionCalibrationStep()
        {
        }

        public void SetConfig(CalibrationViewModel config)
        {

        }
    }
}