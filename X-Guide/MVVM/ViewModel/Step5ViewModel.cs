using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;

using X_Guide.Service.Communication;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase
    {

        private int _jogDistance;
        private CancellationTokenSource cancelJog;
        private readonly CalibrationViewModel _setting;
        private readonly IServerService _serverService;
        private BackgroundService searchClient;

        private Queue<JogCommand> commandQueue = new Queue<JogCommand>();

        VmProcedure p;

        public string JogMode { get; set; } = "TOOL";

        private bool _canJog = false;

  
        private TcpClientInfo _tcpClient;

        private TcpClientInfo TcpClient
        {
            get => _tcpClient; 
        
            set {
              
                _tcpClient = value;
                if (value != null)
                {
                    InitiateJog();
;                    OnJogCanExecuteChanged(true);
                }
            }
        }
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        public ICommand ReportCommand { get; }
        public ICommand JogCommand { get; }
        public ICommand ReconnectCommand { get; set; }

        public int JogDistance
        {
            get { return _jogDistance; }
            set { _jogDistance = value; }
        }

      
        public Step5ViewModel(CalibrationViewModel setting, IServerService serverService)
        {

            ReconnectCommand = new RelayCommand(null);
            JogCommand = new RelayCommand(Jog, CanStartJog);
            _setting = setting;
            _serverService = serverService;
/*            _serverService.ClientDisconnectedEvent += HandleClientDisconnection;*/
            ReconnectCommand = new RelayCommand(testing);

            searchClient = new BackgroundService(SearchForClient);
            searchClient.Start();
           

        }


      
        private void HandleClientDisconnection(object sender, EventArgs e)
        {
            if (cancelJog != null)
            {
                cancelJog.Cancel();
                Application.Current.Dispatcher.Invoke(() => OnJogCanExecuteChanged(false));
                searchClient.Start();
            }
        }

        private void testing(object obj)
        {
            StartJog();
        }

        private void SearchForClient()
        {
            try
            {
                var tcpClient = _serverService.GetConnectedClient().First().Value;

                Application.Current.Dispatcher.Invoke(() => TcpClient = tcpClient);
                searchClient.Stop();

            }
            catch
            {
                Debug.WriteLine("No client found!");
            }

        }

        private async void InitiateJog()
        {
            await Task.Run(()=> StartJog());
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
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(_setting.Speed).SetAcceleration(_setting.Acceleration);
            commandQueue.Enqueue(command);
            
        }

        private bool CanStartJog(object parameter)
        {
            return _canJog;
        }

        private async void StartJog()
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

                await _serverService.SendJogCommand(_tcpClient, commandQueue.Dequeue());
            }
        }

        private void OnJogCanExecuteChanged(bool canJog)
        {
            _canJog = canJog;
            (JogCommand as RelayCommand).RaiseCanExecuteChanged();
        }



        public override ViewModelBase GetNextViewModel()
        {
            return new Step6ViewModel();
        }
    }
}
