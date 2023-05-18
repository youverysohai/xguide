using System;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase, ICalibrationStep
    {
        private readonly CalibrationViewModel _calibration;

        public CalibrationViewModel Calibration
        {
            get
            {
                CheckState();
                return _calibration;
            }
        }

        private void CheckState()
        {
            if (_calibration.XOffset == 0 || _calibration.YOffset == 0 || _calibration.JointRotationAngle == 0) OnStateChanged?.Invoke(false);
            else OnStateChanged?.Invoke(true);
        }

        public Action<bool> OnStateChanged { get; private set; }

        public Step4ViewModel(CalibrationViewModel calibration)
        {
            _calibration = calibration;
        }

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }
    }
}