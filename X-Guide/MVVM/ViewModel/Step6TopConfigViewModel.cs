using AutoMapper;
using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step6TopConfigViewModel : ViewModelBase
    {
        public NinePointCalibrationViewModel NinePoint { get; }

        private readonly IMapper _mapper;
        private readonly IMessenger _messenger;
        public CalibrationViewModel Calibration { get; }


        public RelayCommand StartVision9PointCommand { get; private set; }
        private readonly ICalibrationService _calibrationService;
        public RelayCommand StartCalibrationCommand { get; set; }

        public RelayCommand SaveCalibrationCommand { get; set; }

        private readonly IRepository _repository;

        public Step6TopConfigViewModel(ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger, CalibrationViewModel calibration, IRepository repository, IMapper mapper)
        {
            StartVision9PointCommand = new RelayCommand(StartVision9Point);
            StartCalibrationCommand = new RelayCommand(StartCalibration);
            SaveCalibrationCommand = new RelayCommand(SaveCalibration);
            _repository = repository;
            NinePoint = ninePoint;
            ninePoint.Header = "Vision Calibration";
            _mapper = mapper;
            _messenger = messenger;
            Calibration = calibration;
            _calibrationService = calibrationService;
        }

        private void SaveCalibration()
        {
            Calibration calibration = _repository.Find<Calibration>(q => q.Id.Equals(Calibration.Id)).FirstOrDefault();

            if (calibration is null)
            {
                _repository.Create(_mapper.Map<Calibration>(Calibration));
                //_notifier.ShowSuccess(StrRetriver.Get("SC000"));
            }
            else
            {
                _repository.Update(_mapper.Map<Calibration>(Calibration));
                //_notifier.ShowSuccess($"{Calibration.Name} : {StrRetriver.Get("SC001")}");
            }
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