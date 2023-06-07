using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using ToastNotifications;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3HikViewModel : ViewModelBase
    {
        private readonly IVisionService _visionService;
        private readonly IMessenger _messenger;
        private readonly CalibrationViewModel _calibration;

        public Step3HikViewModel(CalibrationViewModel calibration, IVisionService visionService, StateViewModel viewModelState, Notifier notifier, IMessenger messenger)
        {
            _messenger = messenger;
            _calibration = calibration;
            _visionService = visionService;
        }

        public CalibrationViewModel Calibration
        {
            get
            {
                return _calibration;
            }
        }

        public bool IsProcedureEditable { get; set; } = true;

        public string Procedure
        {
            get
            {
                //patch
                if (_calibration.Procedure == null) OnStateChanged?.Invoke(true);
                else OnStateChanged?.Invoke(true);
                return _procedure;
            }
            set
            {
                _calibration.Procedure = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<VmProcedure> Procedures { get; set; }

        public ObservableCollection<HikVisionViewModel> Visions { get; set; }
        public Action<bool> OnStateChanged { get; private set; }

        private string _procedure => _calibration.Procedure;

        public override async Task<bool> ReadyToDisplay()
        {
            await Dispatcher.CurrentDispatcher.Invoke(() => GetProcedures());
            return true;
        }

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }

        [ExceptionHandlingAspect]
        private async Task GetProcedures()
        {
            Procedures = new ObservableCollection<VmProcedure>(await _visionService.GetAllProcedures());
        }
    }
}