using AutoMapper;

using CommunityToolkit.Mvvm.Messaging;

using System;
using System.Threading.Tasks;
using TcpConnectionHandler.Server;
using ToastNotifications;
using VisionProvider.Interfaces;

//using ToastNotifications.Messages;
using X_Guide.Aspect;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using XGuideSQLiteDB;

namespace X_Guide.MVVM.ViewModel
{
    //TODO: Add tooltip to inform what X and Y Offset is
    internal class Step6ViewModel : ViewModelBase
    {
        public double XMove { get; set; }
        public double YMove { get; set; }

        public CalibrationViewModel Calibration { get; set; }
        public CalibrationViewModel NewCalibration { get; set; }

        public IVisionViewModel VisionView { get; set; }

        private readonly IMessenger _messenger;
        private readonly IServerTcp _serverService;
        private readonly IRepository _repository;
        private readonly ICalibrationService _calibService;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly IVisionService _visionService;
        private bool _canCalibrate = true;

        public bool CanCalibrate
        {
            get { return _canCalibrate; }
            set
            {
                _canCalibrate = value;
                OnPropertyChanged();
            }
        }

        private bool _isFirstCalibResult;

        public bool IsFirstCalibResult
        {
            get { return _isFirstCalibResult; }
            set { _isFirstCalibResult = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }
        public RelayCommand ConfirmCalibDataCommand { get; set; }

        public event EventHandler OnCalibrationChanged;

        public Step6ViewModel(IServerTcp serverService, CalibrationViewModel calibrationConfig, IRepository repository, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService, IMessenger messenger, IVisionViewModel visionView = null)
        {
            _serverService = serverService;
            Calibration = calibrationConfig;
            _repository = repository;
            _calibService = calibService;
            _calibService.SetMotionDelay(Calibration.MotionDelay);
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            VisionView = visionView;
            _messenger = messenger;
            VisionView.SetConfig(calibrationConfig);

            CalibrateCommand = RelayCommand.FromAsyncRelayCommand(Calibrate);
            SaveCommand = RelayCommand.FromAsyncRelayCommand(Save);
            ConfirmCalibDataCommand = new RelayCommand(ConfirmCalibData);
            VisionView.ShowOutputImage();
        }

        private void ConfirmCalibData(object obj)
        {
            if (NewCalibration != null)
            {
                Calibration.CXOffSet = NewCalibration.CXOffSet;
                Calibration.CYOffset = NewCalibration.CYOffset;
                Calibration.CRZOffset = NewCalibration.CRZOffset;
                Calibration.Mm_per_pixel = NewCalibration.Mm_per_pixel;
            }
        }

        private void OnConnectionChange(bool canCalibrate)
        {
            CanCalibrate = canCalibrate;
            CalibrateCommand.OnCanExecuteChanged();
        }

        [ExceptionHandlingAspect]
        private async Task Calibrate(object param)
        {
            int XOffset = Calibration.XOffset;
            int YOffset = Calibration.YOffset;
            CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset, (int)Calibration.JointRotationAngle);
            if (Calibration.Mm_per_pixel != 0.0)
            {
                NewCalibration.CXOffSet = calibrationData.X;
                NewCalibration.CYOffset = calibrationData.Y;
                NewCalibration.CRZOffset = calibrationData.Rz;
                NewCalibration.Mm_per_pixel = calibrationData.mm_per_pixel;
                _isFirstCalibResult = false;
            }
            else
            {
                Calibration.CXOffSet = calibrationData.X;
                Calibration.CYOffset = calibrationData.Y;
                Calibration.CRZOffset = calibrationData.Rz;
                Calibration.Mm_per_pixel = calibrationData.mm_per_pixel;
                _isFirstCalibResult = true;
            }
        }

        [ExceptionHandlingAspect]
        private async Task Save(object param)
        {
            //Calibration calibration = _repository.Find<Calibration>(q => q.Id.Equals(Calibration.Id)).FirstOrDefault();

            //if (calibration is null)
            //{
            //    _repository.Create(_mapper.Map<Calibration>(Calibration));
            //    _notifier.ShowSuccess(StrRetriver.Get("SC000"));
            //}
            //else
            //{
            //    _repository.Update(_mapper.Map<Calibration>(Calibration));
            //    _notifier.ShowSuccess($"{Calibration.Name} : {StrRetriver.Get("SC001")}");
            //}
        }

        public override void Dispose()
        {
            _messenger.Unregister<ConnectionStatusChanged>(this);
            base.Dispose();
        }

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
        }
    }
}