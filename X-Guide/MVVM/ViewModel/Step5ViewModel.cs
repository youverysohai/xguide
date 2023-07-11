﻿/*using VM.Core;*/

using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using ManipulatorTcp;
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
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step5ViewModel : ViewModelBase, IRecipient<ConnectionStatusChanged>
    {
        private readonly CalibrationViewModel _calibrationConfig;

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

        public CalibrationViewModel Calibration => _calibrationConfig;
        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = false;

        public bool CanJog
        {
            get => _canJog;
            private set
            {
                _canJog = value;
                JogCommand.OnCanExecuteChanged();
                if (value) _jogService.Start();
                else _jogService.Stop();

                OnPropertyChanged();
            }
        }

        public bool IsLoading { get; set; } = true;

        public RelayCommand JogCommand { get; }
        public RelayCommand StartJogTrackingCommand { get; }

        public int JogDistance { get; set; }
        public int RotationAngle { get; set; }
        public int TrackedXMove { get; set; }
        public int TrackedYMove { get; set; }

        public Step5ViewModel(CalibrationViewModel calibrationConfig, IServerTcp serverService, IVisionService visionService, IJogService jogService, StateViewModel appState, IMessenger messenger, ICalibrationService calibrationService, IVisionViewModel visionView = null)
        {
            _serverService = serverService;
            _visionService = visionService;
            _calibrationService = calibrationService;
            _calibrationConfig = calibrationConfig;
            AppState = appState;
            _jogService = jogService;
            TestingCommand = new RelayCommand(Initiate9Point);
            VisionView = visionView;
            VisionView?.SetConfig(_calibrationConfig);
            JogCommand = new RelayCommand(Jog, (o) => _canJog);
            StartJogTrackingCommand = new RelayCommand(StartJogTracking);
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

        private void StartJogTracking(object obj)
        {
            if (TrackedXMove != 0) TrackedXMove = 0;
            if (TrackedYMove != 0) TrackedYMove = 0;
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

        private void Jog(object parameter)
        {
            if (JogDistance == 0) JogDistance = 10;
            if (RotationAngle == 0) RotationAngle = 10;
            int x = 0, y = 0, z = 0, rz = 0, rx = 0, ry = 0;

            switch (parameter)
            {
                case "Y+": y = JogDistance; _calibrationConfig.YMove += JogDistance; break;
                case "Y-": y = -JogDistance; _calibrationConfig.YMove -= JogDistance; break;
                case "X+": x = JogDistance; _calibrationConfig.XMove += JogDistance; break;
                case "X-": x = -JogDistance; _calibrationConfig.XMove -= JogDistance; break;
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
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(_calibrationConfig.Speed).SetAcceleration(_calibrationConfig.Acceleration);
            _jogService.Enqueue(command);
        }

        public void Receive(ConnectionStatusChanged message)
        {
            CanJog = message.Value;
        }
    }
}