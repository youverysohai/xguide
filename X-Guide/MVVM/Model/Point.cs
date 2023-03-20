using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlent_Vision_Guided
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", X, Y);
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
            Y /= -pixel_per_mm;
        }
    }
}
