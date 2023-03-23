using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel:ViewModelBase
    {
        CalibrationViewModel _setting;
        private IClientService _clientService;
        private IServerService _serverService;
        

        public ICommand CalibrateCommand { get; set; }
        public Step6ViewModel(CalibrationViewModel setting, IClientService clientService, IServerService serverService)
        {
            _setting = setting;
            _clientService = clientService;
            _serverService = serverService;
            CalibrateCommand = new RelayCommand(Calibrate);
        }

        private void Calibrate(object obj)
        {
            throw new NotImplementedException();
        }

        private async void FindXMoveYMove(int jogDistance, int rotateAngle)
        {

            Point Vis_Center = await _clientService.GetVisCenter();

            TcpClientInfo tcpClientInfo = _serverService.GetConnectedClient().First().Value;
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(jogDistance));
            await Task.Delay(2000);
            Point Vis_Positive = await _clientService.GetVisCenter();
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(-jogDistance));
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetRZ(rotateAngle));
            await Task.Delay(2000);
            Point Vis_Rotate = await _clientService.GetVisCenter();
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetRZ(-rotateAngle));
            double[] MoveOffset = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(MoveOffset[0]).SetY(MoveOffset[1]));


        }
    }
}
