using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {
        private readonly CalibrationViewModel _calibration;

        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        public Step4ViewModel(CalibrationViewModel calibration)
        {
            _calibration = calibration;
            _calibration.JointRotationAngle = 6969;
        }
    }
}