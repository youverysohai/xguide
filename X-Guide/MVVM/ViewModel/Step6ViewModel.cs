using AutoMapper;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;
using VM.Core;
using VMControls.Interface;
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
        #region HalconSpecificParam

        public event EventHandler<HObject> OnImageRecieved;

        public HObject OutputImage { get; set; }
        public Point OutputImageParameter { get; set; }
        public HObject LiveImage { get; set; }
        public RelayCommand LiveImageCommand { get; set; }

        #endregion HalconSpecificParam

        public double XMove { get; set; }
        public double YMove { get; set; }

        public CalibrationViewModel Calibration { get; set; }

        public IVmModule VisProcedure { get; set; }

        public List<VmModule> Modules { get; set; }
        private bool isLive = false;

        private readonly ICalibrationDb _calibDb;
        private readonly ICalibrationService _calibService;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly IVisionService _visionService;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }

        public Step6ViewModel(CalibrationViewModel calibration, ICalibrationDb calibDb, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService)
        {
            Calibration = calibration;
            _calibDb = calibDb;
            _calibService = calibService;
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            CalibrateCommand = RelayCommand.FromAsyncRelayCommand(Calibrate);
            SaveCommand = RelayCommand.FromAsyncRelayCommand(Save);
            VisProcedure = _visionService.GetProcedure(Calibration.Procedure);
            Modules = _visionService.GetModules(VisProcedure as VmProcedure);
            LiveImageCommand = new RelayCommand(ToggleLiveImage);
            ((HalcomVisionService)_visionService).OnImageReturn += Step6ViewModel_OnImageReturn;
            ((HalcomVisionService)_visionService).OnOutputImageReturn += Step6ViewModel_OnOutputImageReturn;
        }

        private void Step6ViewModel_OnOutputImageReturn(object sender, (HObject, object) e)
        {
            OutputImage = e.Item1;
            OutputImageParameter = e.Item2 as Point;
        }

        private void Step6ViewModel_OnImageReturn(object sender, HObject e)
        {
            if (isLive is false) return;
            LiveImage = e;
        }

        private void ToggleLiveImage(object obj)
        {
            isLive = !isLive;
        }

        [ExceptionHandlingAspect]
        private async Task Calibrate(object param)
        {
            int XOffset = (int)Calibration.XOffset;
            int YOffset = (int)Calibration.YOffset;
            CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset, XMove, YMove);
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