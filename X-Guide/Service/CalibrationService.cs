using IMVSCircleFindModuCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.Service
{
    public class CalibrationService : ICalibrationService
    {
        private readonly IVisionService _visionService;
        private readonly IJogService _jogService;
        private IMVSCircleFindModuTool _circleFind;
        private CalibrationViewModel _calibration;
        private double[] calibData;
        private IVmModule VisProcedure;
        public CalibrationService(IVisionService visionService, IJogService jogService) 
        {
            _visionService = visionService;
            _jogService = jogService;
        }


        public async void EyeInHand2DConfig_Calibrate(CalibrationViewModel calibration)
        {
            _calibration = calibration;
            await _visionService.RunProcedure($"{_calibration.Procedure}", true);
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
                return;
            }
        }
        private async Task<double[]> FindXMoveYMove(int jogDistance, int rotateAngle)
        {
            await Task.Delay(1000);
            Point Vis_Center = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(new JogCommand().SetX(jogDistance));

            await Task.Delay(1000);
            Point Vis_Positive = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(new JogCommand().SetX(-jogDistance));
            await _jogService.SendJogCommand(new JogCommand().SetRZ(rotateAngle));

            await Task.Delay(1000);
            Point Vis_Rotate = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(new JogCommand().SetRZ(-rotateAngle));
            double[] XYMove = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

            return XYMove;

        }
        private async Task<(Point[], Point[])> Start9PointCalib()
        {
            int XOffset = (int)_calibration.XOffset;
            int YOffset = (int)_calibration.YOffset;

            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            RobotPoints[4] = new Point(0, 0);
            VisionPoints[4] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[1] = new Point(0, -YOffset);
            VisionPoints[1] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetX(XOffset));
            await Task.Delay(1000);

            RobotPoints[0] = new Point(XOffset, -YOffset);
            VisionPoints[0] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[3] = new Point(XOffset, 0);
            VisionPoints[3] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[6] = new Point(XOffset, YOffset);
            VisionPoints[6] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[7] = new Point(0, YOffset);
            VisionPoints[7] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[8] = new Point(-XOffset, YOffset);
            VisionPoints[8] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[5] = new Point(-XOffset, 0);
            VisionPoints[5] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[2] = new Point(-XOffset, -YOffset);
            VisionPoints[2] = await _visionService.GetVisCenter();


            await _jogService.SendJogCommand(new JogCommand().SetY(YOffset).SetX(XOffset));
            return (VisionPoints, RobotPoints);
        }
    }
}
