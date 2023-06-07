﻿using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;
using X_Guide.Aspect;
using X_Guide.Communication.Service;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    //TODO: Add tooltip to inform what X and Y Offset is
    internal class Step6ViewModel : ViewModelBase
    {
        public double XMove { get; set; }
        public double YMove { get; set; }

        public CalibrationViewModel Calibration { get; set; }

        public IVisionViewModel VisionView { get; set; }

        private readonly IMessenger _messenger;
        private readonly IServerService _serverService;
        private readonly ICalibrationDb _calibDb;
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

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }

        public Step6ViewModel(IServerService serverService, CalibrationViewModel calibrationConfig, ICalibrationDb calibDb, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService, IVisionViewModel visionView, IMessenger messenger)
        {
            _serverService = serverService;
            Calibration = calibrationConfig;
            _calibDb = calibDb;
            _calibService = calibService;
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            VisionView = visionView;
            _messenger = messenger;
            VisionView.SetConfig(calibrationConfig);

            CalibrateCommand = RelayCommand.FromAsyncRelayCommand(Calibrate);
            SaveCommand = RelayCommand.FromAsyncRelayCommand(Save);
            VisionView.ShowOutputImage();
        }

        private void OnConnectionChange(bool canCalibrate)
        {
            CanCalibrate = canCalibrate;
            CalibrateCommand.OnCanExecuteChanged();
        }

        [ExceptionHandlingAspect]
        private async Task Calibrate(object param)
        {
            int XOffset = (int)Calibration.XOffset;
            int YOffset = (int)Calibration.YOffset;
            CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset, (int)Calibration.JointRotationAngle);
            Calibration.CXOffSet = calibrationData.X;
            Calibration.CYOffset = calibrationData.Y;
            Calibration.CRZOffset = calibrationData.Rz;
            Calibration.Mm_per_pixel = calibrationData.mm_per_pixel;
        }

        [ExceptionHandlingAspect]
        private async Task Save(object param)
        {
            if (!await _calibDb.IsExist(Calibration.Id))
            {
                await _calibDb.Add(_mapper.Map<CalibrationModel>(Calibration));
                _notifier.ShowSuccess(StrRetriver.Get("SC000"));
            }
            else
            {
                await _calibDb.Update(_mapper.Map<CalibrationModel>(Calibration));
                _notifier.ShowSuccess($"{Calibration.Name} : {StrRetriver.Get("SC001")}");
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