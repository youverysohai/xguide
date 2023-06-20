using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

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