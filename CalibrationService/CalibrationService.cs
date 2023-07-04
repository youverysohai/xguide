﻿using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ManipulatorTcp;
using System.Diagnostics;
using VisionGuided;
using VisionProvider.Interfaces;
using Point = VisionGuided.Point;

namespace CalibrationProvider
{
    public class CalibrationService : ICalibrationService
    {
        private readonly ManualResetEventSlim _manualResetEvent;
        private readonly ManualResetEventSlim _resetReadySlim;
        private readonly IVisionService _visionService;
        private readonly IJogService _jogService;
        private readonly IMessenger _messenger;
        private readonly SemaphoreSlim _semaphore;
        private int _motionDelay;
        private readonly JogCommand _jogCommand;

        public CalibrationService(IVisionService visionService, IJogService jogService, IMessenger messenger)
        {
            _messenger = messenger;

            _visionService = visionService;
            _jogService = jogService;
            _jogCommand = new JogCommand().SetManipulatorName("Chun");
        }

        private async Task<(double, double)> FindXMoveYMove(int jogDistance, int rotateAngle)
        {
            await Task.Delay(_motionDelay);
            Point Vis_Center = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(jogDistance));

            await Task.Delay(_motionDelay);
            Point Vis_Positive = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetX(-jogDistance));
            await _jogService.SendJogCommand(_jogCommand.SetRZ(rotateAngle));

            await Task.Delay(_motionDelay);
            Point Vis_Rotate = await _visionService.GetVisCenter();

            await _jogService.SendJogCommand(_jogCommand.SetRZ(-rotateAngle));
            double[] XYMove = VisionProcessor.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);

            return (XYMove[0], XYMove[1]);
        }

        public async Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, int JointRotationAngle)
        {
            (double XMove, double YMove) = await FindXMoveYMove(XOffset, JointRotationAngle);
            return await EyeInHand2D_Calibrate(XOffset, YOffset, XMove, YMove);
        }

        public async Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove)
        {
            (Point[] VisionPoint, Point[] RobotPoint) = await EyeInHand9PointAuto(XOffset, YOffset);

            var calibData = VisionProcessor.EyeInHandConfig2D_Calib(VisionPoint, RobotPoint, XMove, YMove, true);
            return new CalibrationData
            {
                X = calibData[0],
                Y = calibData[1],
                Rz = calibData[2],
                mm_per_pixel = calibData[3]
            };
        }

        public Task<CalibrationData> Top2D_Calibrate(int XOffset, int YOffset)
        {
            throw new NotImplementedException();
        }

        public void SetMotionDelay(int motionDelay)
        {
            _motionDelay = motionDelay;
        }

        public async Task<(Point[], Point[])> TopConfig9Point()
        {
            Point[] VisionPoints = new Point[9];
            Point[] RobotPoints = new Point[9];

            for (int i = 0; i < 9; i++)
            {
                await _messenger.Send(new ReadyProceed(true));
                Debug.WriteLine($"Pass through {i}");
                Point? point = _messenger.Send<VisionCenterRequest>();
                if (point is null) throw new Exception();
                VisionPoints[i] = point;
            }

            await _messenger.Send(new ReadyProceed(false));

            for (int i = 0; i < 9; i++)
            {
                Point? point = _messenger.Send<ManipulatorTcpRequest>();
                if (point is null) throw new Exception();
                RobotPoints[i] = point;
            }

            return (VisionPoints, RobotPoints);
        }

        private async Task<(Point[], Point[])> EyeInHand9PointAuto(int XOffset, int YOffset)
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

            int x = 0;
            int y = 0;
            for (int i = 0; i < offsets.GetLength(0); i++)
            {
                RobotPoints[offsets[i, 2]] = new Point(x, y);
                VisionPoints[offsets[i, 2]] = _messenger.Send<VisionCenterRequest>();

                x += offsets[i, 0];
                y += offsets[i, 1];

                await _jogService.SendJogCommand(_jogCommand.SetX(offsets[i, 0]).SetY(offsets[i, 1]));
                await Task.Delay(_motionDelay);
            }

            return (VisionPoints, RobotPoints);
        }
    }

    public class ReadyProceed : AsyncRequestMessage<bool>
    {
        public ReadyProceed(bool ready)
        {
            Ready = ready;
        }

        public bool Ready = false;
    }
}