
using ImageSourceModuleCs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VM.Core;
using VMControls.Interface;
using VMControls.WPF.Release;
/*using VM.Core;*/
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;

using X_Guide.Service.Communication;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase, IDisposable
    {

        private int _jogDistance;
        private CancellationTokenSource cancelJog;
        private readonly CalibrationViewModel _calibration;
        private readonly IJogService _jogService;
        private readonly IServerService _serverService;
        private readonly IVisionService _visionService;

        private IVmModule _visProcedure;

        public IVmModule VisProcedure
        {
            get { return _visProcedure; }
            set { _visProcedure = value;
                OnPropertyChanged();
            }
        }
     


        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = false;
        public bool CanJog
        {
            get => _canJog;
            private set {
                _canJog = value;
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

        public ICommand ReconnectCommand { get; set; }

        public int JogDistance
        {
            get { return _jogDistance; }
            set { _jogDistance = value; }
        }


        public Step5ViewModel(CalibrationViewModel calibration, IServerService serverService, IVisionService visionService, IJogService jogService)
        {
            _serverService = serverService;
            _visionService = visionService;
            _calibration = calibration;
            _jogService = jogService;
    

            ReconnectCommand = new RelayCommand(null);
            JogCommand = new RelayCommand(Jog, (o) => _canJog);
            _serverService.ClientConnectionChange += OnConnectionChange;
           /* RunProcedure();*/
            _jogService.Start();
        }

        private void OnConnectionChange(object sender, bool canJog)
        {
            CanJog = canJog;
            if (canJog) _jogService.Start();
            else _jogService.Stop();
            Application.Current.Dispatcher.Invoke(() =>
            {
                JogCommand.OnCanExecuteChanged();
            });
            
        }

      
        private async void RunProcedure()
        {
            await _visionService.ImportSol(_calibration.Vision.Filepath);
            IVmModule vmProcedure = null;
            try
            {
                vmProcedure = await _visionService.RunProcedure("Live", true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            VisProcedure = vmProcedure;
        }

        private void Jog(object parameter)
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

        public void Dispose()
        {
            _serverService.ClientConnectionChange -= OnConnectionChange;
        }
    }
}
