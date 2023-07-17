//using ToastNotifications.Messages;

using AutoMapper;
using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using TcpConnectionHandler.Server;
using ToastNotifications;
using VisionProvider.Interfaces;
using X_Guide.Aspect;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.MVVM.ViewModel
{
    public class MockViewModel : ViewModelBase
    {
        public CalibrationViewModel Calibration { get; }

        public MockViewModel(CalibrationViewModel calibration)
        {
            Calibration = calibration;
        }
    }

    //TODO: Add tooltip to inform what X and Y Offset is
    [SupportedOSPlatform("windows")]
    internal class Step6ViewModel : ViewModelBase
    {
        public object Mock { get; set; }
        public CalibrationViewModel Calibration { get; set; }
        public CalibrationViewModel NewCalibration { get; set; }

        public List<int> Order { get; set; } = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
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

        private bool _isCalibrationCompleted;

        public bool IsCalibrationCompleted
        {
            get { return _isCalibrationCompleted; }
            set
            {
                _isCalibrationCompleted = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand TestingCommand { get; }
        public RelayCommand CalibrateCommand { get; set; }
        public RelayCommand ConfirmCalibDataCommand { get; set; }

        public event EventHandler OnCalibrationChanged;

        public Step6ViewModel(IServerTcp serverService, CalibrationViewModel calibration, IRepository repository, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService, IMessenger messenger, NinePointCalibrationViewModel ninePoint, IVisionViewModel visionView = null)
        {
            calibration.CalibrationData = new CalibrationData { Y = 7969 };
            Mock = new Step6TopConfigViewModel(calibService, ninePoint, messenger, calibration, repository, mapper);
            _serverService = serverService;
            Calibration = calibration;
            _repository = repository;
            _calibService = calibService;
            _calibService.SetMotionDelay(Calibration.MotionDelay);
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            VisionView = visionView;
            _messenger = messenger;
            VisionView.SetConfig(calibration);
            TestingCommand = new RelayCommand(Testing);
            CalibrateCommand = RelayCommand.FromAsyncRelayCommand(Calibrate);
            SaveCommand = RelayCommand.FromAsyncRelayCommand(Save);
            ConfirmCalibDataCommand = new RelayCommand(ConfirmCalibData);
            VisionView.ShowOutputImage();
        }

        private async void Testing(object obj)
        {
            await _calibService.TopConfig9PointVision(BlockingCall);
        }

        private Task BlockingCall(int index)
        {
            _messenger.Send(new MessageBoxRequest("Do you want to proceed?", BoxState.Normal));

            NinePointState[index] = true;

            return Task.CompletedTask;
        }

        private void ConfirmCalibData(object obj)
        {
            if (NewCalibration != null)
            {
                Calibration.CalibrationData = NewCalibration.CalibrationData;
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
            Calibration.CalibrationData = calibrationData;
            IsCalibrationCompleted = true;
        }

        [ExceptionHandlingAspect]
        private async Task Save(object param)
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