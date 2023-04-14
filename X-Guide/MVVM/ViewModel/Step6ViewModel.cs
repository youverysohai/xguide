using AutoMapper;
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
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {
        private CalibrationViewModel _calibration;
        private IMVSCircleFindModuTool _circleFind;
        private double[] calibData;
        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
            set
            {
                _calibration = value;
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
        public RelayCommand OperationCommand { get; set; }
        private IVisionService _visionService;
        private IServerService _serverService;
        private readonly ICalibrationDb _calibDb;
        private readonly IMapper _mapper;
        private TcpClientInfo _tcpClientInfo;
        public RelayCommand SaveCommand { get; set; }

        public RelayCommand CalibrateCommand { get; set; }



        public Step6ViewModel(CalibrationViewModel calibration, IClientService clientService, IServerService serverService, ICalibrationDb calibDb, IVisionService visionService, IMapper mapper)
        {
            _calibration = calibration;
            _visionService = visionService;
            _serverService = serverService;
            _calibDb = calibDb;
            _mapper = mapper;
            CalibrateCommand = new RelayCommand(Calibrate);
            SaveCommand = new RelayCommand(Save);
            OperationCommand = new RelayCommand(Operation);
            ImportSolution();
        
        }
        private async void ImportSolution()
        {
            await _visionService.ImportSol(_calibration.Vision.Filepath);
            System.Windows.MessageBox.Show("Imported done!");
          
        }
       

        private async void Operation(object parameter)
        {
            await _visionService.RunProcedure("Long", true);
            try
            {
                Point VisCenter = await _visionService.GetVisCenter();
                double[] OperationData = VisionGuided.EyeInHandConfig2D_Operate(VisCenter, calibData);
                OperationData[2] -= 240;
                if (OperationData[2] > 180) OperationData[2] -= 360;
                else if (OperationData[2] <= 180) OperationData[2] += 360;
                await _serverService.SendJogCommand(new JogCommand().SetX(OperationData[0]).SetY(OperationData[1]).SetRZ(OperationData[2]));
                await _serverService.SendJogCommand(new JogCommand().SetZ(-178));
                System.Windows.MessageBox.Show("Press OK to reset machine!");
                await _serverService.ServerWriteDataAsync("RESET");
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception: {ex.Message} | Aborting operation process!");
                await _serverService.ServerWriteDataAsync("RESET");
                return;
            }
        }

        private async void Save(object obj)
        {
            if (!await _calibDb.IsExist(_calibration.Id))
            {
                await _calibDb.Add(_mapper.Map<CalibrationModel>(_calibration));
            }
            else
            {
                await _calibDb.Update(_mapper.Map<CalibrationModel>(_calibration));
            }
            System.Windows.MessageBox.Show("Saved!");
        }

        private async void Calibrate(object obj)
        {
            /* await _serverService.ServerWriteDataAsync("RESET\r\n", _tcpClientInfo.TcpClient.GetStream());*/

    
            await _visionService.RunProcedure($"Circle", true);
            _circleFind = (IMVSCircleFindModuTool)VmSolution.Instance["Circle.Circle Search1"];
            VisProcedure = _circleFind;
            _calibration.XOffset = _calibration.XOffset == 0 ? 10 : _calibration.XOffset;
            _calibration.YOffset = _calibration.YOffset == 0 ? 10 : _calibration.YOffset;

            double[] XYMove;            

            try
            {
               XYMove = await FindXMoveYMove(30, 15);
                (Point[] VisionPoint, Point[] RobotPoint) = await Start9PointCalib();

                calibData = VisionGuided.EyeInHandConfig2D_Calib(VisionPoint, RobotPoint, XYMove[0], XYMove[1], true);

                _calibration.CXOffSet = calibData[0];
                _calibration.CYOffset = calibData[1];
                _calibration.CRZOffset = calibData[2];
                _calibration.Mm_per_pixel = calibData[3];
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception: {ex.Message} | Aborting calibration process!");
                await _serverService.ServerWriteDataAsync("RESET");
                return;
            }



    
        }



        private async Task<(Point[], Point[])> Start9PointCalib()
        {
            int XOffset = (int)_calibration.XOffset;
            int YOffset = (int)_calibration.YOffset;

            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            RobotPoints[4] = new Point(0, 0);
            VisionPoints[4] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[1] = new Point(0, -YOffset);
            VisionPoints[1] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetX(XOffset));
            await Task.Delay(1000);

            RobotPoints[0] = new Point(XOffset, -YOffset);
            VisionPoints[0] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[3] = new Point(XOffset, 0);
            VisionPoints[3] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[6] = new Point(XOffset, YOffset);
            VisionPoints[6] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[7] = new Point(0, YOffset);
            VisionPoints[7] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[8] = new Point(-XOffset, YOffset);
            VisionPoints[8] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[5] = new Point(-XOffset, 0);
            VisionPoints[5] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[2] = new Point(-XOffset, -YOffset);
            VisionPoints[2] = await _visionService.GetVisCenter();


            await _serverService.SendJogCommand(new JogCommand().SetY(YOffset).SetX(XOffset));
            return (VisionPoints, RobotPoints);
        }

        private async Task<double[]> FindXMoveYMove(int jogDistance, int rotateAngle)
        {
            await Task.Delay(1000);
            Point Vis_Center = await _visionService.GetVisCenter();

            await _serverService.SendJogCommand(new JogCommand().SetX(jogDistance));

            await Task.Delay(1000);
            Point Vis_Positive = await _visionService.GetVisCenter();

            await _serverService.SendJogCommand(new JogCommand().SetX(-jogDistance));
            await _serverService.SendJogCommand( new JogCommand().SetRZ(rotateAngle));

            await Task.Delay(1000);
            Point Vis_Rotate = await _visionService.GetVisCenter();

            await _serverService.SendJogCommand(new JogCommand().SetRZ(-rotateAngle));
            double[] XYMove = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

            return XYMove;

        }
    }
}
