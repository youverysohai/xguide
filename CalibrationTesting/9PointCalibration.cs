using System;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using Xlent_Vision_Guided;

namespace CalibrationTest
{
    public class _9PointCalibration
    {
        internal class JogService
        {
            public Task SendJogCommand(JogCommand jogCommand)
            {
                Console.WriteLine(jogCommand.ToString());
                return Task.CompletedTask;
            }
        }

        internal class VisionService
        {
            private readonly int[,] points = new int[9, 2] {
            { 123, 213 },
            { 213, 32 },
            { 213, 213 },
            { 213, 123 },
            { 213, 213 },
            { 11, 67 },
            { 3453, 1435 },
            { 435, 435 },
            { 45, 435 },
        };

            private int i = 0;

            public Task<Point> GetVisCenter()
            {
                var point = new Point(points[i, 0], points[i, 1]);
                if (i == 8) i = 0;
                else i++;
                return Task.Run(() => point);
            }
        }

        private readonly CalibrationViewModel _calibration = new CalibrationViewModel();
        private readonly VisionService _visionService = new VisionService();
        private readonly JogCommand _jogCommand = new JogCommand();
        private readonly JogService _jogService = new JogService();

        public _9PointCalibration()
        {
            _calibration.XOffset = 10;
            _calibration.YOffset = 10;
            _jogCommand.SetManipulatorName("Tester01");
        }

        public async Task<(Point[], Point[])> Start9PointCalib()
        {
            int XOffset = (int)_calibration.XOffset;
            int YOffset = (int)_calibration.YOffset;

            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            RobotPoints[4] = new Point(0, 0);
            VisionPoints[4] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[1] = new Point(0, -YOffset);
            VisionPoints[1] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(XOffset));
            await Task.Delay(1000);

            RobotPoints[0] = new Point(XOffset, -YOffset);
            VisionPoints[0] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[3] = new Point(XOffset, 0);
            VisionPoints[3] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(YOffset));
            await Task.Delay(1000);

            RobotPoints[6] = new Point(XOffset, YOffset);
            VisionPoints[6] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[7] = new Point(0, YOffset);
            VisionPoints[7] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(-XOffset));
            await Task.Delay(1000);

            RobotPoints[8] = new Point(-XOffset, YOffset);
            VisionPoints[8] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[5] = new Point(-XOffset, 0);
            VisionPoints[5] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(-YOffset));
            await Task.Delay(1000);

            RobotPoints[2] = new Point(-XOffset, -YOffset);
            VisionPoints[2] = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetY(YOffset).SetX(XOffset));
            return (VisionPoints, RobotPoints);
        }

        public async Task<(Point[], Point[])> NewStart9PointCalib()
        {
            int XOffset = (int)_calibration.XOffset;
            int YOffset = (int)_calibration.YOffset;

            //int[,] offsets = new int[9, 3] {
            //{0, 0, 4},
            //{0, -YOffset, 1},
            //{XOffset, -YOffset, 0},
            //{XOffset, 0, 3},
            //{XOffset, YOffset, 6},
            //{0, YOffset, 7},
            //{-XOffset, YOffset, 8},
            //{-XOffset, 0, 5},
            //{-XOffset, -YOffset, 2},
            //};

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