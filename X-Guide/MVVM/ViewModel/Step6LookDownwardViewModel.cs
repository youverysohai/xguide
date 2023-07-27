using AutoMapper;
using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step6LookDownwardViewModel : ViewModelBase
    {
        public NinePointCalibrationViewModel NinePoint { get; }

        private readonly IMapper _mapper;
        private readonly IMessenger _messenger;
        public CalibrationViewModel Calibration { get; }

        public RelayCommand StartVision9PointCommand { get; private set; }
        private readonly ICalibrationService _calibrationService;
        public RelayCommand StartCalibrationCommand { get; set; }

        public Step6LookDownwardViewModel(ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger, CalibrationViewModel calibration, IMapper mapper)
        {
            ninePoint.provider = Provider.Vision;
            ninePoint.Header = "Vision Calibration";

            NinePoint = ninePoint;

            _mapper = mapper;
            _messenger = messenger;
            Calibration = calibration;
            _calibrationService = calibrationService;
        }

        private async void StartVision9Point()
        {
            var i = await NinePoint.LookingDownward9PointVision();
            Calibration.VisionPoints = i;
        }

        private async void StartCalibration()
        {
            Calibration.CalibrationData = await _calibrationService.LookingDownward2D_Calibrate(Calibration.VisionPoints, Calibration.RobotPoints);
        }

        private Task BlockingCall(int arg)
        {
            throw new NotImplementedException();
        }
    }
}