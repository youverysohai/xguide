using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlent_Vision_Guided
{
    static class VisionGuided
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

        public static double[] EyeInHandConfig2D_Calib(Point[] VisionCoords, Point[] RobotCoords, double x_move, double y_move, bool conversion)
        {

            double[] x_offset = new double[8];
            double[] y_offset = new double[8];
            double[] theta_offset = new double[8];
            double[] Calib_Data = new double[4];
            double mm_per_pixel = 1;

            if (conversion)
            {
                mm_per_pixel = PixelToMMConversion(VisionCoords, RobotCoords);
            }
           
            int c = 0;
            for (int i = 0; i < 8; i++)
            {
            
                if (i == 4) c++;
            
                EyeInHandConfig2D_get_XYU_Offset(VisionCoords[c], RobotCoords[c], VisionCoords[4], RobotCoords[4], x_move, y_move, ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
                c++; 

            }


            // Median method
            Array.Sort(x_offset);
            Array.Sort(y_offset);
            Array.Sort(theta_offset);
            Calib_Data[0] = (x_offset[3] + x_offset[4]) / 2;
            Calib_Data[1] = (y_offset[3] + y_offset[4]) / 2;
            Calib_Data[2] = (theta_offset[3] + theta_offset[4]) / 2;
            Calib_Data[3] = mm_per_pixel;

            return Calib_Data;
        }
        private static void EyeInHandConfig2D_get_XYU_Offset(Point vision_p1, Point robot_p1, Point vision_p0, Point robot_p0, double x_move, double y_move, ref double x_offset, ref double y_offset, ref double theta_offset)
        {
            {
                double alpha_rad, beta_rad, gamma_rad;
                double rotated_vision_x1, rotated_vision_y1;
                // alpha = vision deviation angle
                // beta = robot deviation angle
                // gamma = deviation angle difference
                /*                Debug.WriteLine($"Vision0: {vision_p0}, Robot0: {robot_p0}, Vision1: {vision_p1}, Robot1: {robot_p1}");
                */
                alpha_rad = Math.Atan2(vision_p0.Y - vision_p1.Y, vision_p0.X - vision_p1.X);   //angle in vision is inverted for this configuration, thus use x0 - x1 , y0 - y1
                beta_rad = Math.Atan2(robot_p1.Y - robot_p0.Y, robot_p1.X - robot_p0.X);

                gamma_rad = alpha_rad - beta_rad;       // angle required for vision frame to align with robot frame


                //rotated vision point x1,y1 with gamma angle, so that both frame aligned, and able to find x and y offset
                rotated_vision_x1 = vision_p1.X * Math.Cos(gamma_rad) + vision_p1.Y * Math.Sin(gamma_rad);
                rotated_vision_y1 = -vision_p1.X * Math.Sin(gamma_rad) + vision_p1.Y * Math.Cos(gamma_rad);

                // offset from robot frame to vision frame
                x_offset = -robot_p1.X - x_move - rotated_vision_x1;
                y_offset = -robot_p1.Y - y_move - rotated_vision_y1;
                /* Debug.WriteLine($"X: {x_offset}, OY:{y_offset}");*/

                theta_offset = -gamma_rad * 180.0 / Math.PI;
                if (theta_offset <= -180.0)
                    theta_offset += 360;
                else if (theta_offset > 180.0)
                    theta_offset -= 360;
            }
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
            Array.Sort(pixel_mm);
            double pixel_per_mm = (pixel_mm[5] + pixel_mm[6]) / 2;

            for (int i = 0; i < VisionCoords.Length; i++)
            {
                VisionCoords[i].PixelConversion(pixel_per_mm);

            }

            return 1/pixel_per_mm;
        }
        #region legacyCode for testing
             public static double[] OldEyeInHandConfig2D_Calib(double[] vision_x_data, double[] vision_y_data, double[] robot_x_data, double[] robot_y_data, double x_move, double y_move, bool conversion)
        {

            // input variable declaration
            double[] Vision_X = new double[9];
            double[] Vision_Y = new double[9];
            double[] Robot_X = new double[9];
            double[] Robot_Y = new double[9];
            // output variable declaration
            double[] Calib_Data = new double[3];
            double[] pixel_distance = new double[12];
            double[] robot_distance = new double[12];
            double pixel_per_mm;
            double[] x_offset = new double[8];
            double[] y_offset = new double[8];
            double[] theta_offset = new double[8];



            if (conversion == true)
            {
                // need do pixel per mm conversion
                pixel_distance[0] = Math.Sqrt(Math.Pow(vision_x_data[1] - vision_x_data[0], 2) + Math.Pow(vision_y_data[1] - vision_y_data[0], 2)); // vision horizontal
                pixel_distance[1] = Math.Sqrt(Math.Pow(vision_x_data[2] - vision_x_data[1], 2) + Math.Pow(vision_y_data[2] - vision_y_data[1], 2));
                pixel_distance[2] = Math.Sqrt(Math.Pow(vision_x_data[4] - vision_x_data[3], 2) + Math.Pow(vision_y_data[4] - vision_y_data[3], 2));
                pixel_distance[3] = Math.Sqrt(Math.Pow(vision_x_data[5] - vision_x_data[4], 2) + Math.Pow(vision_y_data[5] - vision_y_data[4], 2));
                pixel_distance[4] = Math.Sqrt(Math.Pow(vision_x_data[7] - vision_x_data[6], 2) + Math.Pow(vision_y_data[7] - vision_y_data[6], 2));
                pixel_distance[5] = Math.Sqrt(Math.Pow(vision_x_data[8] - vision_x_data[7], 2) + Math.Pow(vision_y_data[8] - vision_y_data[7], 2));

                pixel_distance[6] = Math.Sqrt(Math.Pow(vision_x_data[3] - vision_x_data[0], 2) + Math.Pow(vision_y_data[3] - vision_y_data[0], 2)); // vision vertical
                pixel_distance[7] = Math.Sqrt(Math.Pow(vision_x_data[6] - vision_x_data[3], 2) + Math.Pow(vision_y_data[6] - vision_y_data[3], 2));
                pixel_distance[8] = Math.Sqrt(Math.Pow(vision_x_data[4] - vision_x_data[1], 2) + Math.Pow(vision_y_data[4] - vision_y_data[1], 2));
                pixel_distance[9] = Math.Sqrt(Math.Pow(vision_x_data[7] - vision_x_data[4], 2) + Math.Pow(vision_y_data[7] - vision_y_data[4], 2));
                pixel_distance[10] = Math.Sqrt(Math.Pow(vision_x_data[5] - vision_x_data[2], 2) + Math.Pow(vision_y_data[5] - vision_y_data[2], 2));
                pixel_distance[11] = Math.Sqrt(Math.Pow(vision_x_data[8] - vision_x_data[5], 2) + Math.Pow(vision_y_data[8] - vision_y_data[5], 2));

                robot_distance[0] = Math.Sqrt(Math.Pow(robot_x_data[1] - robot_x_data[0], 2) + Math.Pow(robot_y_data[1] - robot_y_data[0], 2)); // robot horizontal
                robot_distance[1] = Math.Sqrt(Math.Pow(robot_x_data[2] - robot_x_data[1], 2) + Math.Pow(robot_y_data[2] - robot_y_data[1], 2));
                robot_distance[2] = Math.Sqrt(Math.Pow(robot_x_data[4] - robot_x_data[3], 2) + Math.Pow(robot_y_data[4] - robot_y_data[3], 2));
                robot_distance[3] = Math.Sqrt(Math.Pow(robot_x_data[5] - robot_x_data[4], 2) + Math.Pow(robot_y_data[5] - robot_y_data[4], 2));
                robot_distance[4] = Math.Sqrt(Math.Pow(robot_x_data[7] - robot_x_data[6], 2) + Math.Pow(robot_y_data[7] - robot_y_data[6], 2));
                robot_distance[5] = Math.Sqrt(Math.Pow(robot_x_data[8] - robot_x_data[7], 2) + Math.Pow(robot_y_data[8] - robot_y_data[7], 2));

                robot_distance[6] = Math.Sqrt(Math.Pow(robot_x_data[3] - robot_x_data[0], 2) + Math.Pow(robot_y_data[3] - robot_y_data[0], 2)); // robot vertical
                robot_distance[7] = Math.Sqrt(Math.Pow(robot_x_data[6] - robot_x_data[3], 2) + Math.Pow(robot_y_data[6] - robot_y_data[3], 2));
                robot_distance[8] = Math.Sqrt(Math.Pow(robot_x_data[4] - robot_x_data[1], 2) + Math.Pow(robot_y_data[4] - robot_y_data[1], 2));
                robot_distance[9] = Math.Sqrt(Math.Pow(robot_x_data[7] - robot_x_data[4], 2) + Math.Pow(robot_y_data[7] - robot_y_data[4], 2));
                robot_distance[10] = Math.Sqrt(Math.Pow(robot_x_data[5] - robot_x_data[2], 2) + Math.Pow(robot_y_data[5] - robot_y_data[2], 2));
                robot_distance[11] = Math.Sqrt(Math.Pow(robot_x_data[8] - robot_x_data[5], 2) + Math.Pow(robot_y_data[8] - robot_y_data[5], 2));



                double[] pixel_mm = new double[12];
                for (int i = 0; i < 12; i++)
                {
                    pixel_mm[i] = pixel_distance[i] / robot_distance[i];
                }

                //Median method
                Array.Sort(pixel_mm);
                pixel_per_mm = (pixel_mm[5] + pixel_mm[6]) / 2;

            }
            else
            {
                // no do pixel per mm conversion
                pixel_per_mm = 1;

                //Console.WriteLine("2D EyeInHand Pixel per mm = {0}", pixel_per_mm);
            }

            // convert pixel to mm, and add negative to y-data
            for (int i = 0; i < vision_x_data.Length; i++)
            {
                vision_x_data[i] = vision_x_data[i] / pixel_per_mm;
                vision_y_data[i] = vision_y_data[i] / pixel_per_mm;
            }

            Vision_X = vision_x_data;
            Vision_Y = vision_y_data;
            Robot_X = robot_x_data;
            Robot_Y = robot_y_data;



            for (int i = 0; i < 4; i++)
            {
                OldEyeInHandConfig2D_get_XYU_Offset(Vision_X[i], Vision_Y[i], Robot_X[i], Robot_Y[i], Vision_X[4], Vision_Y[4], Robot_X[4], Robot_Y[4], x_move, y_move, ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
            }
            for (int i = 4; i < 8; i++)
            {
                OldEyeInHandConfig2D_get_XYU_Offset(Vision_X[i + 1], Vision_Y[i + 1], Robot_X[i + 1], Robot_Y[i + 1], Vision_X[4], Vision_Y[4], Robot_X[4], Robot_Y[4], x_move, y_move, ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
            }

            // Average method
            //Calib_Data[0] = x_offset.Average();
            //Calib_Data[1] = y_offset.Average();
            //Calib_Data[2] = theta_offset.Average();

            // Median method
            Array.Sort(x_offset);
            Array.Sort(y_offset);
            Array.Sort(theta_offset);
            Calib_Data[0] = (x_offset[3] + x_offset[4]) / 2;
            Calib_Data[1] = (y_offset[3] + y_offset[4]) / 2;
            Calib_Data[2] = (theta_offset[3] + theta_offset[4]) / 2;

            return Calib_Data;
        }

         private static void OldEyeInHandConfig2D_get_XYU_Offset(double vision_x1, double vision_y1, double robot_x1, double robot_y1, double vision_x0, double vision_y0, double robot_x0, double robot_y0, double x_move, double y_move, ref double x_offset, ref double y_offset, ref double theta_offset)
         {
             {
                 double alpha_rad, beta_rad, gamma_rad;
                 double rotated_vision_x1, rotated_vision_y1;
                 // alpha = vision deviation angle
                 // beta = robot deviation angle
               // gamma = deviation angle difference
               // gamma = deviation angle difference

                 alpha_rad = Math.Atan2((vision_y0 - vision_y1), (vision_x0 - vision_x1));   //angle in vision is inverted for this configuration, thus use x0 - x1 , y0 - y1
                 beta_rad = Math.Atan2((robot_y1 - robot_y0), (robot_x1 - robot_x0));

                 gamma_rad = alpha_rad - beta_rad;       // angle required for vision frame to align with robot frame
         

                 //rotated vision point x1,y1 with gamma angle, so that both frame aligned, and able to find x and y offset
                 rotated_vision_x1 = vision_x1 * Math.Cos(gamma_rad) + vision_y1 * Math.Sin(gamma_rad);
                 rotated_vision_y1 = -vision_x1 * Math.Sin(gamma_rad) + vision_y1 * Math.Cos(gamma_rad);

                 // offset from robot frame to vision frame
                 x_offset = -robot_x1 - x_move - rotated_vision_x1;
                 y_offset = -robot_y1 - y_move - rotated_vision_y1;

               

                 theta_offset = -gamma_rad * 180.0 / Math.PI;
                 if (theta_offset <= -180.0)
                     theta_offset += 360;
                 else if (theta_offset > 180.0)
                     theta_offset -= 360;
             }
         }
        #endregion


        static double RadToDeg(double rad)
        {
            return rad * (180 / Math.PI);


        }
        static double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }









    }
}
