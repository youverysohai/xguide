
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
    internal class Step5ViewModel : ViewModelBase
    {

        private int _jogDistance;
        private CancellationTokenSource cancelJog;
        private readonly CalibrationViewModel _calibration;
        private readonly IServerService _serverService;
        private readonly IVisionService _visionService;

        private Queue<JogCommand> commandQueue = new Queue<JogCommand>();

        private IVmModule _visProcedure;

        public IVmModule VisProcedure
        {
            get { return _visProcedure; }
            set { _visProcedure = value;
                OnPropertyChanged();
            }
        }

        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = true;

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


        public Step5ViewModel(CalibrationViewModel calibration, IServerService serverService, IVisionService visionService)
        {
            _serverService = serverService;
            _visionService = visionService;
            _calibration = calibration;

            RunProcedure();
            InitiateJog();

            ReconnectCommand = new RelayCommand(null);
            JogCommand = new RelayCommand(Jog, (o) => _canJog);



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
     

        private void HandleClientDisconnection(object sender, EventArgs e)
        {
            if (cancelJog != null)
            {
                cancelJog.Cancel();
                Application.Current.Dispatcher.Invoke(() => OnJogCanExecuteChanged(false));
            }
        }

        private async void InitiateJog()
        {
            await Task.Run(()=>StartJog());
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
            commandQueue.Enqueue(command);

        }

        private async Task StartJog()
        {
            cancelJog = new CancellationTokenSource();
            CancellationToken ct = cancelJog.Token;
            while (!ct.IsCancellationRequested)
            {
                if (commandQueue.Count <= 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                await _serverService.SendJogCommand(commandQueue.Dequeue());
            }
        }

        private void OnJogCanExecuteChanged(bool canJog)
        {
            _canJog = canJog;
            JogCommand.OnCanExecuteChanged();
        }



   
    }
}
