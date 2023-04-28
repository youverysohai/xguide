using IMVSCircleFindModuCs;
using System;
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
        private readonly IJogService _jogService;
        private readonly IVisionService _visionService;
        private CalibrationViewModel _calibration;
        private IMVSCircleFindModuTool _circleFind;
        private JogCommand _jogCommand;
        private double[] calibData;
        private IVmModule VisProcedure;

        public CalibrationService(IVisionService visionService, IJogService jogService)
        {
            _visionService = visionService;
            _jogService = jogService;
        }

        /// <inheritdoc/>

        public async void EyeInHand2DConfig_Calibrate(CalibrationViewModel calibration)
        {
            _calibration = calibration;
            _jogCommand = new JogCommand().SetManipulatorName(_calibration?.Manipulator.Name);

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

            await _jogService.SendJogCommand(_jogCommand.SetX(jogDistance));

            await Task.Delay(1000);
            Point Vis_Positive = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(-jogDistance));
            await _jogService.SendJogCommand(_jogCommand.SetRZ(rotateAngle));

            await Task.Delay(1000);
            Point Vis_Rotate = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetRZ(-rotateAngle));
            double[] XYMove = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

            return XYMove;
        }

        private async Task<(Point[], Point[])> Start9PointCalib()
        {
            int XOffset = (int)_calibration.XOffset;
            int YOffset = (int)_calibration.YOffset;

            int[,] offsets = new int[9, 3] {
            {0, -YOffset,4},
            {XOffset,0, 1},
            {0, YOffset,0},
            {0,YOffset,3},
            {-XOffset, 0,6},
            {-XOffset, 0,7},
            { 0, -YOffset ,8},
            {0,-YOffset, 5},
            {XOffset, YOffset, 2},
            };

            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            int delayMs = 1000;
            int x = 0;
            int y = 0;
            for (int i = 0; i < offsets.GetLength(0); i++)
            {
                RobotPoints[offsets[i, 2]] = new Point(x, y);
                VisionPoints[offsets[i, 2]] = await _visionService.GetVisCenter();

                x += offsets[i, 0];
                y += offsets[i, 1];

                await _jogService.SendJogCommand(_jogCommand.SetX(offsets[i, 0]).SetY(offsets[i, 1]));
                await Task.Delay(delayMs);
            }

            return (VisionPoints, RobotPoints);
        }
    }
}