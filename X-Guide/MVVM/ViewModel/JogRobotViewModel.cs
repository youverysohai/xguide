
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.Service;
using X_Guide.Communication.Service;
using X_Guide.Service.Communication;
using Serilog;

namespace X_Guide.MVVM.ViewModel
{
    class JogRobotViewModel:ViewModelBase
    {
        private readonly IJogService _jogService;
        private readonly IServerService _serverService;
        private bool _canJog;
        public int JogDistance { get; set; }
        public int JogSpeed { get; set; }
        public int JogAccel { get; set; }

        public RelayCommand JogCommand { get; }
        public bool CanJog
        {
            get { return _canJog; }
            set { _canJog = value; OnPropertyChanged(); }
        }


        public JogRobotViewModel(IServerService serverService, IJogService jogService) {
            _jogService = jogService;
            _serverService = serverService;
            _serverService.ClientConnectionChange += OnConnectionChange;
            JogCommand = new RelayCommand(Jog, (o) => _canJog);
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
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(JogSpeed).SetAcceleration(JogAccel);
            _jogService.Enqueue(command);
        }

        private void OnConnectionChange(object sender, bool canJog)
        {
            CanJog = canJog;
            Application.Current.Dispatcher.Invoke(() =>
            {
                JogCommand.OnCanExecuteChanged();
            });
        }
    }
}
