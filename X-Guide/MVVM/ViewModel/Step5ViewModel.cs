/*using VM.Core;*/

using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using ManipulatorTcp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public CalibrationViewModel Calibration { get; }
        public int JogDistance { get; set; }
        private readonly IJogService _jogService;
        public int RotationAngle { get; set; }
        public int TrackedXMove { get; set; }
        public int TrackedYMove { get; set; }
        public RelayCommand JogCommand { get; set; }

        public JogImplementationViewModel(CalibrationViewModel calibration, IJogService jogService)
        {
            Calibration = calibration;
            _jogService = jogService;
            JogCommand = new RelayCommand(Jog);
            _jogService.Start();
        }

        private void Jog(object parameter)
        {
            if (JogDistance == 0) JogDistance = 10;
            if (RotationAngle == 0) RotationAngle = 10;
            int x = 0, y = 0, z = 0, rz = 0, rx = 0, ry = 0;

            switch (parameter)
            {
                case "Y+": y = JogDistance; Calibration.YMove += JogDistance; break;
                case "Y-": y = -JogDistance; Calibration.YMove -= JogDistance; break;
                case "X+": x = JogDistance; Calibration.XMove += JogDistance; break;
                case "X-": x = -JogDistance; Calibration.XMove -= JogDistance; break;
                case "Z+": z = JogDistance; break;
                case "Z-": z = -JogDistance; break;
                case "RZ+": rz = RotationAngle; break;
                case "RZ-": rz = -RotationAngle; break;
                case "RX+": rx = RotationAngle; break;
                case "RX-": rx = RotationAngle; break;
                case "RY+": ry = RotationAngle; break;
                case "RY-": ry = RotationAngle; break;
                default: break;
            }
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(Calibration.Speed).SetAcceleration(Calibration.Acceleration);
            _jogService.Enqueue(command);
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

        public Step5ViewModel(CalibrationViewModel calibrationConfig, IServerTcp serverService, IVisionService visionService, IJogService jogService, StateViewModel appState, IMessenger messenger, ICalibrationService calibrationService, IVisionViewModel visionView = null)
        {
            JogImplementation = new JogImplementationViewModel(calibrationConfig, jogService);
            _serverService = serverService;
            _visionService = visionService;
            _calibrationService = calibrationService;
            AppState = appState;
            _jogService = jogService;
            TestingCommand = new RelayCommand(Initiate9Point);
            StartJogTrackingCommand = new RelayCommand(null);
            messenger.Register(this);
            VisionView?.StartLiveImage();
        }

        private async void Initiate9Point(object obj)
        {
            Debug.WriteLine("Start calib");
            var i = await _calibrationService.LookingDownward9PointManipulator(BlockingCall);
        }

        private Task BlockingCall(int index)
        {
            System.Windows.MessageBox.Show("Next?");
            NinePointState[index] = true;
            return Task.CompletedTask;
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