using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3ViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly ViewModelState _viewModelState;
        private readonly IVisionDb _visionDb;
        private readonly IVisionService _visionService;
        private readonly CalibrationViewModel _calibration;
        private ManualResetEventSlim _manual;

        public Step3ViewModel(CalibrationViewModel calibration, IVisionService visionService, IVisionDb visionDb, IMapper mapper, ViewModelState viewModelState, Notifier notifier)
        {
            _calibration = calibration;
            _visionService = visionService;
            _visionDb = visionDb;
            _mapper = mapper;
            _notifier = notifier;
            _viewModelState = viewModelState;
            InitView();
        }

        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        public bool IsProcedureEditable { get; set; } = false;

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

        public VisionViewModel Vision
        {
            get { return _vision; }
            set
            {
                _calibration.Vision = value;
                OnPropertyChanged();
                if (value != null)
                {
                    UpdateProcedures();
                }
            }
        }

        public ObservableCollection<VisionViewModel> Visions { get; set; }
        private string _procedure => _calibration.Procedure;
        private VisionViewModel _vision => _calibration.Vision;

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

        private async Task GetProcedures()
        {
            try
            {
                IsProcedureEditable = false;
                if (Vision is null) return;
                await _visionService.ImportSol(Vision.Filepath);
                Procedures = new ObservableCollection<VmProcedure>(_visionService.GetAllProcedures());
                IsProcedureEditable = true;
            }
            catch (Exception ex)
            {
                if (ex is CriticalErrorException) throw ex;
                Vision = null;
                Procedure = null;
                _notifier.ShowError(ex.Message);
            }
        }

        [ExceptionHandlingAspect]
        private async Task GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetAll();
            Visions = new ObservableCollection<VisionViewModel>(models.Select(x => _mapper.Map<VisionViewModel>(x)));
        }

        private async void InitView()
        {
            await GetVisions();
            await GetProcedures();
            _manual?.Set();
        }

        private async void UpdateProcedures()
        {
            _viewModelState.IsLoading = true;
            await GetProcedures();
            _viewModelState.IsLoading = false;
        }
    }
}