﻿using AutoMapper;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;
using X_Guide.Aspect;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {
        public double XMove { get; set; }
        public double YMove { get; set; }

        public CalibrationViewModel Calibration { get; set; }

        public IVisionViewModel VisionView { get; set; }

        private readonly ICalibrationDb _calibDb;
        private readonly ICalibrationService _calibService;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly IVisionService _visionService;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }

        public Step6ViewModel(CalibrationViewModel calibrationConfig, ICalibrationDb calibDb, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService, IVisionViewModel visionView)
        {
            Calibration = calibrationConfig;
            _calibDb = calibDb;
            _calibService = calibService;
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            VisionView = visionView;
            VisionView.SetConfig(calibrationConfig);
            CalibrateCommand = RelayCommand.FromAsyncRelayCommand(Calibrate);
            SaveCommand = RelayCommand.FromAsyncRelayCommand(Save);
        }

        [ExceptionHandlingAspect]
        private async Task Calibrate(object param)
        {
            VisionView.ShowOutputImage();
            int XOffset = (int)Calibration.XOffset;
            int YOffset = (int)Calibration.YOffset;
            CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset);
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
    }
}