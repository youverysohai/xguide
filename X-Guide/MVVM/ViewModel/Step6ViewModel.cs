namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {
        public CalibrationViewModel Calibration { get; set; }
        public HTuple WindowHandle { get; set; }
        public HTuple OutputHandle { get; set; }

        public IVmModule VisProcedure { get; set; }

        public List<VmModule> Modules { get; set; }
        bool isLive = false;

        public RelayCommand OperationCommand { get; set; }

        private readonly ICalibrationDb _calibDb;
        private readonly ICalibrationService _calibService;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly IVisionService _visionService;
        public event EventHandler<HObject> OnImageRecieved;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }
        public RelayCommand LiveImageCommand { get; set; }

        public Step6ViewModel(CalibrationViewModel calibration, ICalibrationDb calibDb, ICalibrationService calibService, IMapper mapper, Notifier notifier, IVisionService visionService)
        {
            Calibration = calibration;
            _calibDb = calibDb;
            _calibService = calibService;
            _mapper = mapper;
            _notifier = notifier;
            _visionService = visionService;
            CalibrateCommand = new RelayCommand(Calibrate);
            SaveCommand = new RelayCommand(Save);
            VisProcedure = _visionService.GetProcedure(Calibration.Procedure);
            Modules = _visionService.GetModules(VisProcedure as VmProcedure);
            LiveImageCommand = new RelayCommand(LiveImage);
            ((HalcomVisionService)_visionService).OnImageReturn += Step6ViewModel_OnImageReturn;
        }

        private void Step6ViewModel_OnImageReturn(object sender, HObject e)
        {
            if (isLive is false || WindowHandle is null) return;
            HOperatorSet.DispObj(e, WindowHandle);
        }

        private async void LiveImage(object obj)
        {
            isLive = !isLive;
        }

        private async void Calibrate(object obj)
        {
            var image = _visionService.GetImage();
            Point result = await _visionService.GetVisCenter();
            HOperatorSet.DispImage(image, OutputHandle);
            HOperatorSet.SetColor(OutputHandle, "blue");
            HOperatorSet.DispCross(OutputHandle, result.X, result.Y, 20, 0);
            MessageBox.Show(result.ToString());
            //int XOffset = (int)Calibration.XOffset;
            //int YOffset = (int)Calibration.YOffset;
            //double XMove = 10;
            //double YMove = 20;
            //CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset, XMove, YMove);
            //Calibration.CXOffSet = calibrationData.X;
            //Calibration.CYOffset = calibrationData.Y;
            //Calibration.CRZOffset = calibrationData.Rz;
            //Calibration.Mm_per_pixel = calibrationData.mm_per_pixel;
            //_calibService.EyeInHand2DConfig_Calibrate(Calibration);
        }

        private async void Save(object obj)
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