using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using ToastNotifications;
using ToastNotifications.Core;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;
using X_Guide.VisionMaster;
using ToastNotifications.Messages;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3ViewModel : ViewModelBase
    {

        private ManualResetEventSlim _manual;
        private CalibrationViewModel _calibration;
        private readonly IVisionService _visionService;
        private readonly IVisionDb _visionDb;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;
        private readonly ViewModelState _viewModelState;

        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        private string _procedure => _calibration.Procedure;

        public string Procedure
        {
            get { return _procedure; }
            set
            {
                _calibration.Procedure = value;
                OnPropertyChanged();
            }
        }
        public bool IsProcedureEditable { get; set; } = false;
        private VisionViewModel _vision => _calibration.Vision;
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

        private async void UpdateProcedures()
        {
  
            _viewModelState.IsLoading = true;
            await GetProcedures();
            _viewModelState.IsLoading = false;
        }


        public ObservableCollection<VisionViewModel> Visions { get; set; }

        public ObservableCollection<string> Procedures { get; set; }

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


        private async void InitView()
        {
            await GetVisions();
            await GetProcedures();
            _manual?.Set();

        }
        private async Task GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetAll();
            Visions = new ObservableCollection<VisionViewModel>(models.Select(x => _mapper.Map<VisionViewModel>(x)));

        }

        private async Task GetProcedures()
        {
            IsProcedureEditable = false;
            if (Vision is null) return;
            try
            {
                await _visionService.ImportSol(Vision.Filepath);

                Procedures = new ObservableCollection<string>(_visionService.GetProcedureNames());
                IsProcedureEditable = true;
            }
            catch(Exception ex)
            {
                Vision = null;
                Procedure = null;
                _notifier.ShowError(ex.Message);
            }
            
        }



    }
}
    