using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Aspect;

/*using VM.Core;*/

using X_Guide.Communication.Service;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;

using X_Guide.Service.Communication;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase, IRecipient<ConnectionStatusChanged>
    {
        private readonly CalibrationViewModel _calibrationConfig;

        public object AppState { get; }

        private readonly IJogService _jogService;
        private readonly IServerService _serverService;
        private readonly IVisionService _visionService;
        private ManualResetEventSlim _manual;

        public IVisionViewModel VisionView
        {
            get
            {
                if (_visionService != null)
                    visionView.StartLiveImage();
                return visionView;
            }
            set => visionView = value;
        }

        public List<VmModule> Modules { get; private set; }

        public IVmModule Module { get; set; }

        public CalibrationViewModel Calibration => _calibrationConfig;
        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = false;
        private IVisionViewModel visionView;

        public bool CanJog
        {
            get => _canJog;
            private set
            {
                _canJog = value;
                if (value) _jogService.Start();
                else _jogService.Stop();
                JogCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public bool IsLoading { get; set; } = true;

        public RelayCommand JogCommand { get; }

        public int JogDistance { get; set; }
        public int RotationAngle { get; set; }

        public Step5ViewModel(CalibrationViewModel calibrationConfig, IServerService serverService, IVisionService visionService, IJogService jogService, IVisionViewModel visionView, StateViewModel appState, IMessenger messenger)
        {
            _serverService = serverService;
            _visionService = visionService;
            _calibrationConfig = calibrationConfig;
            AppState = appState;
            _jogService = jogService;
            VisionView = visionView;
            VisionView.SetConfig(_calibrationConfig);
            JogCommand = new RelayCommand(Jog, (o) => _canJog);

            messenger.Register(this);
            VisionView.StartLiveImage();
        }

        public async void InitView()
        {
            try
            {
                await RunProcedure();
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

        [ExceptionHandlingAspect]
        private async Task RunProcedure()
        {
            IVmModule procedure = await _visionService.RunProcedure(_calibrationConfig.Procedure, false);
            Modules = _visionService.GetModules(procedure as VmProcedure);
        }

        private void Jog(object parameter)
        {
            if (JogDistance == 0) JogDistance = 10;
            if (RotationAngle == 0) RotationAngle = 10;
            int x = 0, y = 0, z = 0, rz = 0, rx = 0, ry = 0 ;

            switch (parameter)
            {
                case "Y+": y = JogDistance; break;
                case "Y-": y = -JogDistance; break;
                case "X+": x = JogDistance; break;
                case "X-": x = -JogDistance; break;
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

        public override void Dispose()
        {
            base.Dispose();
        }

        public void Receive(ConnectionStatusChanged message)
        {
            CanJog = message.Value;
        }
    }
}