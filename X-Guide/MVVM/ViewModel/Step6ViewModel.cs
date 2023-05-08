namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {
        public double XMove { get; set; }


        public double YMove { get; set; }

        public CalibrationViewModel Calibration { get; set; }
        public HTuple WindowHandle { get; set; }
        public HTuple OutputHandle { get; set; }
        public HObject Image { get; set; }

        public IVmModule VisProcedure { get; set; }

        public List<VmModule> Modules { get; set; }
        private bool isLive = false;

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
            ((HalcomVisionService)_visionService).OnOutputImageReturn += Step6ViewModel_OnOutputImageReturn;
        }

        private void Step6ViewModel_OnOutputImageReturn(object sender, (HObject, object) e)
        {
            Point point = e.Item2 as Point;
        }

        private void Step6ViewModel_OnImageReturn(object sender, HObject e)
        {
            if (isLive is false || WindowHandle is null) return;
            Image = e;
        }

        private async void LiveImage(object obj)
        {
            isLive = !isLive;
        }

        private async void Calibrate(object obj)
        {
            int XOffset = (int)Calibration.XOffset;
            int YOffset = (int)Calibration.YOffset;
            Debug.WriteLine(XMove + "   " + YMove);

            CalibrationData calibrationData = await _calibService.EyeInHand2D_Calibrate(XOffset, YOffset, XMove, YMove);
            Calibration.CXOffSet = calibrationData.X;
            Calibration.CYOffset = calibrationData.Y;

            Calibration.CRZOffset = calibrationData.Rz;
            Calibration.Mm_per_pixel = calibrationData.mm_per_pixel;
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