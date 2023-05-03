using System;

namespace X_Guide
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Angle { get; set; }

        public bool IsFound { get; set; } = false;

        public Point()
        {
            X = 0;
            Y = 0;
            Angle = 0;
        }

        public Point(double x, double y, double angle = 0)
        {
            X = x;
            Y = y;
            Angle = angle;
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}, θ: {2}", X, Y, Angle);
        }

        public double CalculateDistance(Point point)
        {
            return Math.Sqrt((X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y));
        }

        public double CalculateAtanRad(Point point)
        {
            return Math.Atan2(Y - point.Y, X - point.X);
        }

        public void PixelConversion(double pixel_per_mm)
        {
            X /= pixel_per_mm;
            Y /= pixel_per_mm;
        }
    }
}