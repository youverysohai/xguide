using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {
        private readonly CalibrationViewModel _calibration;
        private readonly IMessenger _messenger;

        public CalibrationViewModel Calibration
        {
            get
            {
                Debug.WriteLine(_calibration.XOffset);
                CheckState();
                Debug.WriteLine("CheckState");
                return _calibration;
            }
        }

        private void CheckState()
        {
            if (_calibration.XOffset == 0 || _calibration.YOffset == 0 || _calibration.JointRotationAngle == 0) _messenger.Send(new CalibrationStateChanged(PageState.Disable));
            else _messenger.Send(new CalibrationStateChanged(PageState.Enable));
        }

        public Step4ViewModel(CalibrationViewModel calibration, IMessenger messenger)
        {
            _calibration = calibration;
            _messenger = messenger;
        }
    }
}