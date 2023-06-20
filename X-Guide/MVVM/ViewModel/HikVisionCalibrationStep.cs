using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VM.Core;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class HikVisionCalibrationStep : ViewModelBase, IRecipient<ReadyRequest>, IVisionCalibrationStep
    {
        private readonly IMessenger _messenger;
        private CalibrationViewModel _calibration;
        private readonly IVisionService _visionService;

        public CalibrationViewModel Calibration
        {
            get
            {
                return _calibration;
            }
            set
            {
                _calibration = value;
            }
        }

        public HikVisionCalibrationStep(IVisionService visionService, IMessenger messenger)
        {
            _messenger = messenger;
            _visionService = visionService;
            _messenger.Register(this);
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

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }

        private async Task GetProcedures()
        {
            await Task.Delay(5000);
        }

        public void SetConfig(CalibrationViewModel config)
        {
            Calibration = config;
        }

        void IRecipient<ReadyRequest>.Receive(ReadyRequest message)
        {
            Procedures = new ObservableCollection<VmProcedure>(_visionService.GetAllProcedures().GetAwaiter().GetResult());
            message.Reply(true);
        }
    }
}