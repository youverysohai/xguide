using System;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
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
        private readonly JogCommand _jogCommand = new JogCommand().SetManipulatorName("Ghost");

        public CalibrationService(IVisionService visionService, IJogService jogService)
        {
            _visionService = visionService;
            _jogService = jogService;
        }

        public async Task<CalibrationData> Calibrate(CalibrationViewModel config)
        {
            (double, double, double) calibData = (0, 0, 0);

            if (config.XMove == 0 || config.YMove == 0)
            {
                (config.XMove, config.YMove) = await FindXMoveYMove((int)config.XOffset, (int)config.JointRotationAngle);
            }

            (Point[] VisionPoint, Point[] RobotPoint) = await Start9PointCalib((int)config.XOffset, (int)config.YOffset);

            switch (config.Orientation)
            {
                case (int)Orientation.EyeOnHand: calibData = VisionGuided.EyeInHand2DConfig_Calib(VisionPoint, RobotPoint, config.XMove, config.YMove); break;
                case (int)Orientation.LookDownward:
                case (int)Orientation.LookUpward: calibData = VisionGuided.TopConfig_Calib(VisionPoint, RobotPoint); break;
                case (int)Orientation.MountedOnJoint2: throw new NotImplementedException();
                case (int)Orientation.MountedOnJoint5: throw new NotImplementedException();
                default: return new CalibrationData();
            }

            return new CalibrationData
            {
                X = calibData.Item1,
                Y = calibData.Item2,
                Rz = calibData.Item3,
                mm_per_pixel = VisionGuided.PixelToMMConversion(VisionPoint, RobotPoint),
            };
        }

        private async Task<(double, double)> FindXMoveYMove(int jogDistance, int rotateAngle)
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

            return (XYMove[0], XYMove[1]);
        }

        private async Task<(Point[], Point[])> Start9PointCalib(int XOffset, int YOffset)
        {
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