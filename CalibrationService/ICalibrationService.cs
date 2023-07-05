﻿using VisionGuided;

namespace CalibrationProvider
{
    public struct CalibrationData
    {
        public double X;
        public double Y;
        public double Rz;
        public double mm_per_pixel;
    }

    public interface ICalibrationService
    {
        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, int JointRotationAngle);

        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove);

        Task<CalibrationData> LookingDownward2D_Calibrate(Func<int, Task> action);

        void SetMotionDelay(int motionDelay);

        public Task<(Point[], Point[])> TopConfig9Point(Func<int, Task> action);
    }
}