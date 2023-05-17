using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal interface IVisionViewModel
    {
        void SetConfig(CalibrationViewModel calibrationViewModel);

        void StartLiveImage();

        void ShowOutputImage();
    }
}