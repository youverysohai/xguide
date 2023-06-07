using System.Threading.Tasks;
using VisionGuided;
using X_Guide.MVVM.Model;
using X_Guide.Service.Communication;
using X_Guide.VisionMaster;

namespace X_Guide.Service
{
    public class CalibrationService : ICalibrationService
    {
        private readonly IJogService _jogService;
        private readonly IVisionService _visionService;
        private readonly JogCommand _jogCommand = new JogCommand().SetManipulatorName("Chun");

        public CalibrationService(IVisionService visionService, IJogService jogService)
        {
            _visionService = visionService;
            _jogService = jogService;
        }

        public async Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove)
        {
            (Point[] VisionPoint, Point[] RobotPoint) = await Start9PointCalib(XOffset, YOffset);

            var calibData = VisionProcessor.EyeInHandConfig2D_Calib(VisionPoint, RobotPoint, XMove, YMove, true);
            return new CalibrationData
            {
                X = calibData[0],
                Y = calibData[1],
                Rz = calibData[2],
                mm_per_pixel = calibData[3]
            };
        }

        public async Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, int JointRotationAngle)
        {
            (double XMove, double YMove) = await FindXMoveYMove(XOffset, JointRotationAngle);
            return await EyeInHand2D_Calibrate(XOffset, YOffset, XMove, YMove);
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
            double[] XYMove = VisionProcessor.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

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