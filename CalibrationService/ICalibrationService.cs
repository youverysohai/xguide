﻿using VisionGuided;

namespace CalibrationProvider
{
    public interface ICalibrationService
    {
        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove);

        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, int JointRotationAngle);

        Task<CalibrationData> LookingDownward2D_Calibrate(Func<int, Task> action);

        Task<CalibrationData> LookingDownward2D_Calibrate(Point[] VisionPoints, Point[] RobotPoints);

        Task<Point[]> LookingDownward9PointManipulator(Func<int, Task> BlockingCall);

        Task<Point[]> LookingDownward9PointVision(Func<int, Task> BlockingCall);

        void SetMotionDelay(int motionDelay);

        Task<(Point[], Point[])> TopConfig9PointVision(Func<int, Task> action);

        Task<Point[]> LookingDownward9Point(Func<int, Task> BlockingCall, Provider provider);
    }
}