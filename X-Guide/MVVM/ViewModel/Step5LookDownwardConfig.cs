using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step5LookDownwardConfig : ViewModelBase
    {

        public NinePointCalibrationViewModel NinePoint { get; set; }

        private readonly CalibrationViewModel _calibration;
        public CalibrationViewModel Calibration => _calibration; 


        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public RelayCommand NextCommand { get; set; }
        public RelayCommand StartCommand { get; set; }

        public Step5LookDownwardConfig(NinePointCalibrationViewModel ninePoint, CalibrationViewModel calibration)
        {
            
            NinePoint = ninePoint;
            NinePoint.provider = Provider.Manipulator;
            _calibration = calibration;
            ninePoint.Header = "Camera Look Downward";
            StartCommand = new RelayCommand(Start9Point);
        }

        private async void Start9Point()
        {
            _calibration.RobotPoints = await NinePoint.LookingDownward9PointManipulator();
        }
    }
}