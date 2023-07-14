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
    internal class Step5TopConfigViewModel
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;
        private readonly ManualResetEventSlim manual;
        public NinePointCalibrationViewModel NinePoint { get; set; }

        private readonly CalibrationViewModel _calibration;

        public JogControllerViewModel JogController { get; set; }
        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public RelayCommand NextCommand { get; set; }
        public RelayCommand StartCommand { get; set; }

        public Step5TopConfigViewModel(JogControllerViewModel controller, ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger, CalibrationViewModel calibration)
        {
            controller.Calibration = calibration;
            NinePoint = ninePoint;
            _calibration = calibration;
            _calibrationService = calibrationService;
            _messenger = messenger;
            JogController = controller;
            ninePoint.Header = "Hello chun";
            StartCommand = new RelayCommand(Start9Point);
        }

        private async void Start9Point()
        {
            _calibration.RobotPoints = await NinePoint.LookingDownward9PointManipulator();
        }
    }
}