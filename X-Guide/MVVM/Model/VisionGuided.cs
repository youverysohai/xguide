using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using X_Guide;

namespace Xlent_Vision_Guided
{
    internal static class VisionGuided
    {
        //===========================================================================================================================================
        // 2D EYE IN HAND CONFIGURATION
        //===========================================================================================================================================
        public static double[] FindEyeInHandXYMoves(Point Vis_Center, Point Vis_Positive, Point Vis_Rotate, int jogDistance, int rotateAngle)
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

            Debug.WriteLine($"NewX_Move = {X_Move}, NewY_Move = {Y_Move}\n\n\n");

            return new double[] { X_Move, Y_Move };
        }

        public static (double, double, double) TopConfig_Calib(Point[] VisionCoords, Point[] RobotCoords)
        {
            return EyeInHand2DConfig_Calib(VisionCoords, RobotCoords);
        }

        public static (double, double, double) EyeInHand2DConfig_Calib(Point[] VisionCoords, Point[] RobotCoords, double x_move = 0, double y_move = 0)
        {
            double rotated_vision_x1, rotated_vision_y1;
            Point visionCenter = VisionCoords[4];
            Point robotCenter = RobotCoords[4];
            List<double> x_offsets = new List<double>();
            List<double> y_offsets = new List<double>();
            List<double> theta_offsets = new List<double>();

            int c = 0;
            for (int i = 0; i < 8; i++)
            {
                if (i == 4) c++;
                double alpha_rad, beta_rad, gamma_rad;
                Point vision_p1 = VisionCoords[c];
                Point robot_p1 = RobotCoords[c];
                c++;

                alpha_rad = Math.Atan2(visionCenter.Y - vision_p1.Y, visionCenter.X - vision_p1.X);
                beta_rad = Math.Atan2(robot_p1.Y - robotCenter.Y, robot_p1.X - robotCenter.X);
                gamma_rad = alpha_rad - beta_rad;

                rotated_vision_x1 = vision_p1.X * Math.Cos(gamma_rad) + vision_p1.Y * Math.Sin(gamma_rad);
                rotated_vision_y1 = -vision_p1.X * Math.Sin(gamma_rad) + vision_p1.Y * Math.Cos(gamma_rad);

                x_offsets.Add(-robot_p1.X + x_move - rotated_vision_x1);
                y_offsets.Add(-robot_p1.Y + y_move - rotated_vision_y1);

                double theta_offset;
                theta_offset = -gamma_rad * 180.0 / Math.PI;
                if (theta_offset <= -180.0)
                    theta_offset += 360;
                else if (theta_offset > 180.0)
                    theta_offset -= 360;
                theta_offsets.Add(theta_offset);
            }

            return (Median(x_offsets), Median(y_offsets), Median(theta_offsets));
        }

        public static double PixelToMMConversion(Point[] VisionCoords, Point[] RobotCoords)
        {
            double[] pixel_distance = new double[12];
            double[] robot_distance = new double[12];
            int counter = 0;
            for (int i = 0; i < 6; i++)
            {
                pixel_distance[i] = VisionCoords[counter + 1].CalculateDistance(VisionCoords[counter]);
                robot_distance[i] = RobotCoords[counter + 1].CalculateDistance(RobotCoords[counter]);
                if (i % 2 == 0) counter++;
                else counter += 2;
            }

            counter = 0;
            for (int i = 6; i < 12; i++)
            {
                pixel_distance[i] = VisionCoords[counter + 3].CalculateDistance(VisionCoords[counter]);
                robot_distance[i] = RobotCoords[counter + 3].CalculateDistance(RobotCoords[counter]);
                counter++;
            }

            double[] pixel_mm = new double[12];

            for (int i = 0; i < 12; i++)
            {
                pixel_mm[i] = pixel_distance[i] / robot_distance[i];
            }

            //Median method
            double pixel_per_mm = Median(pixel_mm);

            for (int i = 0; i < VisionCoords.Length; i++)
            {
                VisionCoords[i].PixelConversion(pixel_per_mm);
            }

            return 1 / pixel_per_mm;
        }

        public static double[] EyeInHandConfig2D_Operate(Point VisCenter, double[] Calib_Data)
        {
            double transformed_vision_x, transformed_vision_y;
            double calib_theta_rad;
            double[] Operate_Data = new double[3];

            // convert vision pixel to mm
            VisCenter.X *= Calib_Data[3];
            VisCenter.Y *= Calib_Data[3];

            /* vision_capture_y = -vision_capture_y;*/

            // invert angle sign
            /*VisCenter.Angle = -VisCenter.Angle;*/
            calib_theta_rad = Calib_Data[2] * Math.PI / 180.0;

            //rotate vision frame first
            transformed_vision_x = VisCenter.X * Math.Cos(-calib_theta_rad) + VisCenter.Y * Math.Sin(-calib_theta_rad);
            transformed_vision_y = -VisCenter.X * Math.Sin(-calib_theta_rad) + VisCenter.Y * Math.Cos(-calib_theta_rad);
            //translation transformation

            Operate_Data[0] = Calib_Data[0] + transformed_vision_x;
            Operate_Data[1] = Calib_Data[1] + transformed_vision_y;
            Operate_Data[2] = Calib_Data[2] + VisCenter.Angle;

            if (Operate_Data[2] <= -180.0)
                Operate_Data[2] += 360;
            else if (Operate_Data[2] > 180.0)
                Operate_Data[2] -= 360;
            Debug.WriteLine("X:{0}, Y;{1}, Theta:{2}", Operate_Data[0], Operate_Data[1], Operate_Data[2]);
            return Operate_Data;
        }

        private static double RadToDeg(double rad)
        {
            return rad * (180 / Math.PI);
        }

        private static double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        private static double Median(IEnumerable<double> array)
        {
            var data = array.ToList();
            double median;
            int count = data.Count;
            if (count % 2 == 0)
            {
                median = (data[count / 2] + data[(count / 2) - 1]) / 2.0;
            }
            else
            {
                median = data[(count - 1) / 2];
            }
            return median;
        }
    }
}