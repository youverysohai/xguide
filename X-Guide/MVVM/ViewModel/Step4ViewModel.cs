using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB.Models;

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
                CheckState();
                Debug.WriteLine("CheckState");
                return _calibration;
            }
            
        }
      
        private void CheckState()
        {
            bool isCalibrationValid = _calibration.XOffset > 0 && _calibration.JointRotationAngle > 0 && _calibration.YOffset > 0;


            if (_calibration.Orientation == Orientation.LookDownward)
            {
                isCalibrationValid = true;
            }
            else if (_calibration.Manipulator.Type == ManipulatorType.GantrySystemWR)
            {
                isCalibrationValid = isCalibrationValid || (_calibration.XOffset > 0 && _calibration.YOffset > 0);
            }

            _messenger.Send(new CalibrationStateChanged(isCalibrationValid ? PageState.Enable : PageState.Disable));
        }

        public Step4ViewModel(CalibrationViewModel calibration, IMessenger messenger)
        {
            _calibration = calibration;
            _messenger = messenger;
        }
    }
}