using IMVSCircleFindModuCs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {
        private CalibrationViewModel _setting;
        private IMVSCircleFindModuTool _circleFind;
        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
                OnPropertyChanged();
            }
        }
        private IVmModule _visProcedure;

        public IVmModule VisProcedure
        {
            get { return _visProcedure; }
            set
            {
                _visProcedure = value;
                OnPropertyChanged();
            }
        }

        private IClientService _clientService;
        private IServerService _serverService;
        private readonly TcpClientInfo _tcpClientInfo;
        public event EventHandler<IMVSCircleFindModuTool> BindProcedure;

        public ICommand CalibrateCommand { get; set; }
        public Step6ViewModel(CalibrationViewModel setting, IClientService clientService, IServerService serverService)
        {
            Setting = setting;
            _clientService = clientService;
            _serverService = serverService;

            try { clientService.ConnectServer(); }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            _tcpClientInfo = _serverService.GetConnectedClient().First().Value;
            _serverService.ServerWriteDataAsync("RESET\r\n", _tcpClientInfo.TcpClient.GetStream());
            ((VmProcedure)VmSolution.Instance["Circle"]).ContinuousRunEnable = true;
            _circleFind = (IMVSCircleFindModuTool)VmSolution.Instance["Circle.Circle Search1"];
            VisProcedure = _circleFind;
            /*        _circleFind.Run();*/
            CalibrateCommand = new RelayCommand(Calibrate);
        }
        /*     private async Task SaveRenderImage()
             {

                 _circleFind.Run();
                 await Task.Delay(1000);
             }*/

        private async void Calibrate(object obj)
        {

            List<double> XMoveList = new List<double>();
            List<double> YMoveList = new List<double>();
            Setting.XOffset = Setting.XOffset == 0 ? 10 : Setting.XOffset;
            Setting.YOffset = Setting.YOffset == 0 ? 10 : Setting.YOffset;

            //for (int i = 0; i < 5; i++)
            //{
            //    double[] xyMove = await FindXMoveYMove(20, 3);
            //    XMoveList.Add(xyMove[0]);
            //    YMoveList.Add(xyMove[1]);
            //}
            //XMoveList.Sort();
            //YMoveList.Sort();
            //XMoveList.ForEach(x => Debug.Write(x + ","));
            //Debug.WriteLine("");
            //YMoveList.ForEach(x => Debug.Write(x + ","));
            //double[] XYMove =
            //{
            //           XMoveList[2],
            //           YMoveList[2],
            //       }; 

            double[] XYMove = await FindXMoveYMove(30, 15);


            /*            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(XYMove[0]).SetY(XYMove[1]).SetZ(-180));
                        await Task.Delay(10000);
                        await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(-XYMove[0]).SetY(-XYMove[1]).SetZ(180));*/

            (Point[] VisionPoint, Point[] RobotPoint) = await Start9PointCalib();
            double[] calibData = VisionGuided.EyeInHandConfig2D_Calib(VisionPoint, RobotPoint, XYMove[0], XYMove[1], true);

            Setting.CXOffSet = Math.Round(calibData[0], 2);
            Setting.CYOffset = Math.Round(calibData[1], 2);
            Setting.CRZOffset = Math.Round(calibData[2], 2);
            Setting.Mm_per_pixel = Math.Round(calibData[3], 2);
            /*        await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(calibData[0]).SetY(calibData[1]).SetRZ(calibData[2]));
                    await Task.Delay(2000);*/
            await _clientService.GetVisCenter();
        }

        private async Task<(Point[], Point[])> Start9PointCalib()
        {
            int XOffset = Setting.XOffset;
            int YOffset = Setting.YOffset;

            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            RobotPoints[4] = new Point(0, 0);
            VisionPoints[4] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[1] = new Point(0, -YOffset);
            VisionPoints[1] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(XOffset));
            await Task.Delay(1000);

            RobotPoints[0] = new Point(XOffset, -YOffset);
            VisionPoints[0] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[3] = new Point(XOffset, 0);
            VisionPoints[3] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[6] = new Point(XOffset, YOffset);
            VisionPoints[6] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[7] = new Point(0, YOffset);
            VisionPoints[7] = await _clientService.GetVisCenter();
            await Task.Delay(3000);

            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[8] = new Point(-XOffset, YOffset);
            VisionPoints[8] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[5] = new Point(-XOffset, 0);
            VisionPoints[5] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[2] = new Point(-XOffset, -YOffset);
            VisionPoints[2] = await _clientService.GetVisCenter();


            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetY(YOffset).SetX(XOffset));
            return (VisionPoints, RobotPoints);
        }

        private async Task<double[]> FindXMoveYMove(int jogDistance, int rotateAngle)
        {
            await Task.Delay(1000);
            Point Vis_Center = await _clientService.GetVisCenter();

            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(jogDistance));

            await Task.Delay(1000);
            Point Vis_Positive = await _clientService.GetVisCenter();

            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(-jogDistance));
            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetRZ(rotateAngle));

            await Task.Delay(1000);
            Point Vis_Rotate = await _clientService.GetVisCenter();

            await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetRZ(-rotateAngle));
            double[] XYMove = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

            return XYMove;

        }
    }
}
