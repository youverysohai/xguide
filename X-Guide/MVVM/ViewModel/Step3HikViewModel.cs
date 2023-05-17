using AutoMapper;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ToastNotifications;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3HikViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly ViewModelState _viewModelState;
        private readonly HikVisionService _visionService;
        private readonly CalibrationViewModel _calibration;
        private ManualResetEventSlim _manual;

        public Step3HikViewModel(CalibrationViewModel calibration, IVisionService visionService, IVisionDb visionDb, IMapper mapper, ViewModelState viewModelState, Notifier notifier)
        {
            _calibration = calibration;
            _visionService = (HikVisionService)visionService;
            _mapper = mapper;
            _notifier = notifier;
            _viewModelState = viewModelState;
            InitView();
        }

        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        public bool IsProcedureEditable { get; set; } = true;

        public string Procedure
        {
            get { return _procedure; }
            set
            {
                _calibration.Procedure = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<VmProcedure> Procedures { get; set; }

        public ObservableCollection<HikVisionViewModel> Visions { get; set; }
        private string _procedure => _calibration.Procedure;

        public override bool ReadyToDisplay()
        {
            if (!_isLoaded)
            {
                using (_manual = new ManualResetEventSlim(false))
                {
                    _manual.Wait();
                    _isLoaded = true;
                }
            }
            return _isLoaded;
        }

        [ExceptionHandlingAspect]
        private async Task GetProcedures()
        {
            Procedures = new ObservableCollection<VmProcedure>(await _visionService.GetAllProcedures());
        }

        private async void InitView()
        {
            await GetProcedures();
            _manual?.Set();
        }

        private void UpdateProcedures()
        {
            _viewModelState.IsLoading = true;
            GetProcedures();
            _viewModelState.IsLoading = false;
        }
    }
}