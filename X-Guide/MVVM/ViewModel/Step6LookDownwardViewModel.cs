using AutoMapper;
using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using VisionGuided;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

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

        private readonly IRepository _repository;

        public Step6LookDownwardViewModel(ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger, CalibrationViewModel calibration, IRepository repository, IMapper mapper)
        {
            Calibration = calibration;

            ninePoint.provider = Provider.Vision;
            ninePoint.Header = "Vision Calibration";
            _repository = repository;
            NinePoint = ninePoint;
            //NinePoint.NinePoint = Calibration.VisionPoints;
            _mapper = mapper;
            _messenger = messenger;
            _calibrationService = calibrationService;
        }

        private async void StartVision9Point()
        {
            //var i = await NinePoint.LookingDownward9PointVision();
            //Calibration.VisionPoints = new ObservableCollection<Point>(i);
        }

        private async void StartCalibration()
        {
            Calibration.CalibrationData = await _calibrationService.LookingDownward2D_Calibrate(Calibration.VisionPoints.ToArray(), Calibration.RobotPoints.Select(x=> x).ToArray());
        }

        private Task BlockingCall(int arg)
        {
            throw new NotImplementedException();
        }
    }
}