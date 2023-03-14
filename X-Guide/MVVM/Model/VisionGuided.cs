using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlent_Vision_Guided
{
    class VisionGuided
    {
        // input variable declaration
        private double[] Vision_X = new double[9];
        private double[] Vision_Y = new double[9];
        private double[] Robot_X = new double[9];
        private double[] Robot_Y = new double[9];
        // output variable declaration
        private double[] Calib_Data = new double[3];


        Point[] VisionCoords = new Point[9];
        Point[] RobotCoords = new Point[9];


        // scaling variable declaration
        private double pixel_per_mm;
        private double[] robot_distance = new double[12];
        // calibration calculation variable declaration
        private double[] x_offset = new double[8];
        private double[] y_offset = new double[8];
        private double[] theta_offset = new double[8];


        //Operation Variable
        private double[] Operate_Data = new double[3];

        //===========================================================================================================================================
        // 2D EYE IN HAND CONFIGURATION
        //===========================================================================================================================================
        public void FindEyeInHandXYMoves(Point Vis_Center, Point Vis_Positive)
        {

            double MoveDist = 10;
            double RotateAngle = 3;

            //calculate angle differences between vis and robot frame 
            double U_Vis2Robot = Math.Atan2(Vis_Center.Y - Vis_Positive.Y, Vis_Center.X - Vis_Positive.X);

            double mm_per_pixel = MoveDist / Vis_Center.CalculateDistance(Vis_Positive);

            Console.WriteLine($"Rotation matrix = {U_Vis2Robot * 180 / Math.PI}");

            Point Vis_Rotate = new Point();

            double Vis_RotateDistance = Vis_Center.CalculateDistance(Vis_Rotate);
            double End_To_Obj_Dist = Vis_RotateDistance * Math.Sin(RotateAngle / 2) / Math.Sin(RotateAngle);

            //Rotation
            double RotationAngleOffset = U_Vis2Robot - RotateAngle;
            Point RotatedFrame_Point_Center = new Point
            {
                X = Vis_Center.X * Math.Cos(RotationAngleOffset) + Vis_Center.X * Math.Sin(RotationAngleOffset),
                Y = -Vis_Center.X * Math.Sin(RotationAngleOffset) + Vis_Center.Y * Math.Cos(RotationAngleOffset),
            };

            Point RotatedFrame_Point_Rotate = new Point
            {
                X = Vis_Rotate.X * Math.Cos(RotationAngleOffset) + Vis_Rotate.X * Math.Sin(RotationAngleOffset),
                Y = -Vis_Rotate.X * Math.Sin(RotationAngleOffset) + Vis_Rotate.Y * Math.Cos(RotationAngleOffset),
            };

            double XDif = RotatedFrame_Point_Rotate.X - RotatedFrame_Point_Center.X;
            double YDif = RotatedFrame_Point_Rotate.Y - RotatedFrame_Point_Center.Y;

            double Seg_Offset = 0;

            if (YDif >= 0 && XDif >= 0) Seg_Offset = 90;
            else if (YDif >= 0 && XDif < 0) Seg_Offset = 180;
            else if (YDif < 0 && XDif < 0) Seg_Offset = 270;

            Console.WriteLine($"Segment Offset = {Seg_Offset}");

            double End_To_Obj_Angle = Seg_Offset - (RotateAngle / 2) + Math.Atan2(Math.Abs(YDif), Math.Abs(XDif));

            double X_Move = End_To_Obj_Dist * Math.Cos(End_To_Obj_Angle);
            double Y_Move = End_To_Obj_Dist * Math.Sin(End_To_Obj_Angle);
            Console.WriteLine($"X_Move = {X_Move}, Y_Move = {Y_Move}");

        }









    }
}
