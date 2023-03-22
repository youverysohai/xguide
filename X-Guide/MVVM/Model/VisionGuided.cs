using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlent_Vision_Guided
{
    class VisionGuided
    {


        //===========================================================================================================================================
        // 2D EYE IN HAND CONFIGURATION
        //===========================================================================================================================================
        public double[] FindEyeInHandXYMoves(Point Vis_Center, Point Vis_Positive, Point Vis_Rotate, int jogDistance, int rotateAngle)
        {

            //calculate angle differences between vis and robot frame 
            double U_Vis2Robot = Math.Atan2(Vis_Center.Y - Vis_Positive.Y, Vis_Center.X - Vis_Positive.X);
            Debug.WriteLine($"Rotation Matrix : {RadToDeg(U_Vis2Robot)}");
            double pixelDistance = Vis_Center.CalculateDistance(Vis_Positive);
            Debug.WriteLine($"Pixel Distance : {pixelDistance}");
            double mm_per_pixel = jogDistance / pixelDistance;

            double rotationAngle = DegToRad(rotateAngle);
            Debug.WriteLine($"Rotation Angle : {rotationAngle}");

            Debug.WriteLine($"mm_per pixel = {mm_per_pixel}");






            double Vis_RotateDistance = Vis_Center.CalculateDistance(Vis_Rotate);
            Debug.WriteLine($"Rotate Distance = {Vis_RotateDistance}");
            double End_To_Obj_Dist = Vis_RotateDistance / Math.Sin(rotationAngle) * Math.Sin(DegToRad(90) - rotationAngle / 2);


            //Rotation
            double RotationAngleOffset = U_Vis2Robot - rotationAngle;
            Debug.WriteLine("Rotation offset = " + RotationAngleOffset);
            Point RotatedFrame_Point_Center = new Point
            {
                X = Vis_Center.X * Math.Cos(RotationAngleOffset) + Vis_Center.Y * Math.Sin(RotationAngleOffset),
                Y = -Vis_Center.X * Math.Sin(RotationAngleOffset) + Vis_Center.Y * Math.Cos(RotationAngleOffset),
            };

            Debug.WriteLine($"For rotated center point= {RotatedFrame_Point_Center}");

            Point RotatedFrame_Point_Rotate = new Point
            {
                X = Vis_Rotate.X * Math.Cos(RotationAngleOffset) + Vis_Rotate.Y * Math.Sin(RotationAngleOffset),
                Y = -Vis_Rotate.X * Math.Sin(RotationAngleOffset) + Vis_Rotate.Y * Math.Cos(RotationAngleOffset),
            };
            Debug.WriteLine($"For rotated rotation point= {RotatedFrame_Point_Rotate}");

            double XDif = RotatedFrame_Point_Rotate.X - RotatedFrame_Point_Center.X;
            double YDif = RotatedFrame_Point_Rotate.Y - RotatedFrame_Point_Center.Y;
            Debug.WriteLine($"XDif: {XDif}, YDif : {YDif}");

            double End_To_Obj_Deg;
            double atan2 = RadToDeg(Math.Atan2(Math.Abs(YDif), Math.Abs(XDif)));


            if (YDif >= 0 && XDif >= 0)
            {
                End_To_Obj_Deg = 90 - RadToDeg(rotationAngle / 2) + atan2;
            }
            else if (YDif >= 0 && XDif < 0)
            {
                End_To_Obj_Deg = 270 - RadToDeg(rotationAngle / 2) - atan2;
            }
            else if (YDif < 0 && XDif < 0)
            {
                End_To_Obj_Deg = 270 - RadToDeg(rotationAngle / 2) + atan2;

            }
            else
            {
                End_To_Obj_Deg = 90 - RadToDeg(rotationAngle / 2) - atan2;
            }

            double End_To_Obj_Rad = DegToRad(End_To_Obj_Deg);
            Debug.WriteLine($"End_To_Obj_Angle = {End_To_Obj_Deg}");
            Debug.WriteLine($"End-To-Object Distance = {End_To_Obj_Dist * mm_per_pixel}");
            double X_Move = End_To_Obj_Dist * Math.Cos(End_To_Obj_Rad) * mm_per_pixel;
            double Y_Move = End_To_Obj_Dist * Math.Sin(End_To_Obj_Rad) * mm_per_pixel;
            Point OriginalPoint = new Point()
            {
                X = 89.7725777133691,
                Y = 16.5052745624083,
            };

            Debug.WriteLine($"\nOldX_Move = {OriginalPoint.X}, Old_Y_Move={OriginalPoint.Y}");
            Debug.WriteLine($"NewX_Move = {X_Move}, NewY_Move = {Y_Move}\n\n\n");

            return new double[] { X_Move, Y_Move };
        }

        double RadToDeg(double rad)
        {
            return rad * (180 / Math.PI);


        }

        double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }

 







    }
}
