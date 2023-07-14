/*using VM.Core;*/

using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using ManipulatorTcp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using TcpConnectionHandler.Server;
using VisionProvider.Interfaces;
using VM.Core;
using VMControls.Interface;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class JogImplementationViewModel : ViewModelBase
    {
        public string Header { get; set; } = "Manipulator Top Config";
        public CalibrationViewModel Calibration { get; }

        private readonly IJogService _jogService;
        public int TrackedXMove { get; set; }
        public int TrackedYMove { get; set; }
        public RelayCommand JogCommand { get; set; }

        public JogImplementationViewModel(CalibrationViewModel calibration, IJogService jogService)
        {
            Calibration = calibration;
        }

        private void StartJogTracking(object obj)
        {
            if (TrackedXMove != 0) TrackedXMove = 0;
            if (TrackedYMove != 0) TrackedYMove = 0;
        }
    }

    [SupportedOSPlatform("windows")]
    internal class Step5ViewModel : ViewModelBase, IRecipient<ConnectionStatusChanged>
    {
        public object Step5CalibrationModule { get; set; }
        public JogImplementationViewModel JogImplementation { get; set; }
        public object AppState { get; }

        private readonly IJogService _jogService;
        private readonly IServerTcp _serverService;
        private readonly IVisionService _visionService;
        private readonly ICalibrationService _calibrationService;
        private ManualResetEventSlim _manual;
        private IVisionViewModel _visionView;
        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public RelayCommand TestingCommand { get; set; }

        public IVisionViewModel VisionView
        {
            get
            {
                if (_visionService != null)
                    _visionView?.StartLiveImage();
                return _visionView;
            }
            set => _visionView = value;
        }

        public List<VmModule> Modules { get; private set; }

        public IVmModule Module { get; set; }
        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = false;

        public bool CanJog
        {
            get => _canJog;
            private set
            {
                _canJog = value;

                if (value) _jogService.Start();
                else _jogService.Stop();

                OnPropertyChanged();
            }
        }

        public bool IsLoading { get; set; } = true;

        public RelayCommand StartJogTrackingCommand { get; }

        public Step5ViewModel(CalibrationViewModel calibration, IServerTcp serverService, IVisionService visionService, IJogService jogService, StateViewModel appState, IMessenger messenger, ICalibrationService calibrationService, JogControllerViewModel controller, NinePointCalibrationViewModel ninePoint, IVisionViewModel visionView = null)
        {
            Step5CalibrationModule = new Step5TopConfigViewModel(controller, calibrationService, ninePoint, messenger, calibration);

            JogImplementation = new JogImplementationViewModel(calibration, jogService);
            _serverService = serverService;
            _visionService = visionService;
            _calibrationService = calibrationService;
            AppState = appState;
            _jogService = jogService;
            StartJogTrackingCommand = new RelayCommand(null);
            messenger.Register(this);
            VisionView?.StartLiveImage();
        }

        public async void InitView()
        {
            try
            {
                _jogService.Start();
            }
            catch
            {
                _canDisplayViewModel = false;
            }
            _manual?.Set();
        }

        public override async Task<bool> ReadyToDisplay()
        {
            using (_manual = new ManualResetEventSlim(false))
            {
                InitView();
                return await Task.FromResult(true);
            }
        }

        public void Receive(ConnectionStatusChanged message)
        {
            CanJog = message.Value;
        }
    }
}