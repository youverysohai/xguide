using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VM.Core;
using VMControls.Interface;

/*using VM.Core;*/

using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;

using X_Guide.Service.Communication;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase
    {
        private readonly CalibrationViewModel _calibration;
        private readonly IJogService _jogService;
        private readonly IServerService _serverService;
        private readonly IVisionService _visionService;
        private ManualResetEventSlim _manual;

        public List<VmModule> Modules { get; private set; }

        public IVmModule Module { get; set; }

        public CalibrationViewModel Calibration => _calibration;
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

        private bool _isLoading = true;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReportCommand { get; }
        public RelayCommand JogCommand { get; }
        public int JogDistance { get; set; }

        public Step5ViewModel(CalibrationViewModel calibration, IServerService serverService, IVisionService visionService, IJogService jogService)
        {
            _serverService = serverService;
            _visionService = visionService;
            _calibration = calibration;
            _jogService = jogService;

            JogCommand = new RelayCommand(Jog, (o) => _canJog);
            _serverService.ClientConnectionChange += OnConnectionChange;

            InitView();
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
            _manual.Set();
        }

        public override bool ReadyToDisplay()
        {
            using (_manual = new ManualResetEventSlim(false))
            {
                _manual.Wait();
                return _canDisplayViewModel;
            }
        }

        private void OnConnectionChange(object sender, bool canJog)
        {
            CanJog = canJog;
            Application.Current.Dispatcher.Invoke(() =>
            {
                JogCommand.OnCanExecuteChanged();
            });
        }

        private async Task RunProcedure()
        {
            try
            {
                IVmModule procedure = await _visionService.RunProcedure(_calibration.Procedure, false);
                Modules = _visionService.GetModules(procedure as VmProcedure);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void Jog(object parameter)
        {
            if (JogDistance == 0) JogDistance = 10;
            int x = 0, y = 0, z = 0, rz = 0;

            switch (parameter)
            {
                case "Y+": y = JogDistance; break;
                case "Y-": y = -JogDistance; break;
                case "X+": x = JogDistance; break;
                case "X-": x = -JogDistance; break;
                case "Z+": z = JogDistance; break;
                case "Z-": z = -JogDistance; break;
                case "RZ+": rz = JogDistance; break;
                case "RZ-": rz = -JogDistance; break;
                default: break;
            }
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(_calibration.Speed).SetAcceleration(_calibration.Acceleration);
            _jogService.Enqueue(command);
        }

        public new void Dispose()
        {
            _serverService.ClientConnectionChange -= OnConnectionChange;
            base.Dispose();
        }
    }
}