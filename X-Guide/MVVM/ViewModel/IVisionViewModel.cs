using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    public interface IVisionViewModel
    {
        void SetConfig(CalibrationViewModel calibrationViewModel);

        void StartLiveImage();

        void ShowOutputImage();

        void StopLiveImage();
    }
}