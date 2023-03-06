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






        public VisionGuided(double[] vision_x_data, double[] vision_y_data, double[] robot_x_data, double[] robot_y_data)
        {
            for (int i = 0; i < vision_x_data.Length; i++)
            {
                VisionCoords[i] = new Point
                {
                    X = vision_x_data[i],
                    Y = vision_y_data[i],
                };

                RobotCoords[i] = new Point
                {
                    X = robot_x_data[i],
                    Y = robot_y_data[i],
                };
            }
            pixel_per_mm = 1;
            Calib_Data = new double[] { 0, 0, 0 };

        }


        //===========================================================================================================================================
        // TOP CONFIGURATION
        //===========================================================================================================================================


        public double[] TopConfig_Calib(bool conversion)
        {

            int counter = 0;

            if (conversion)
            {
                PixelToMMConversion();
            }

            counter = 0;
            for (int i = 0; i < 8; i++)
            {
                TopConfig_get_XYU_Offset(VisionCoords[counter], RobotCoords[counter], VisionCoords[4], RobotCoords[4], ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
                if (i == 3) counter++;
                counter++;
            }

            // Median method
            Array.Sort(x_offset);
            Array.Sort(y_offset);
            Array.Sort(theta_offset);
            Calib_Data[0] = (x_offset[3] + x_offset[4]) / 2;
            Calib_Data[1] = (y_offset[3] + y_offset[4]) / 2;
            Calib_Data[2] = (theta_offset[3] + theta_offset[4]) / 2;

            return Calib_Data;
        }

        private static void TopConfig_get_XYU_Offset(Point vision_p0, Point robot_p0, Point vision_p1, Point robot_p1, ref double x_offset, ref double y_offset, ref double theta_offset)
        {
            double alpha_rad, beta_rad, gamma_rad;
            double rotated_vision_x1, rotated_vision_y1;
            // alpha = vision deviation angle
            // beta = robot deviation angle
            // gamma = deviation angle difference
            alpha_rad = vision_p0.CalculateAtanRad(vision_p1);

            beta_rad = robot_p0.CalculateAtanRad(robot_p1);
            gamma_rad = alpha_rad - beta_rad;       // angle required for vision frame to align with robot frame


            //rotated vision point x1,y1 with gamma angle, so that both frame aligned, and able to find x and y offset
            rotated_vision_x1 = vision_p0.X * Math.Cos(gamma_rad) + vision_p0.Y * Math.Sin(gamma_rad);
            rotated_vision_y1 = -vision_p0.X * Math.Sin(gamma_rad) + vision_p0.Y * Math.Cos(gamma_rad);

            // offset from robot frame to vision frame
            x_offset = robot_p0.X - rotated_vision_x1;
            y_offset = robot_p0.Y - rotated_vision_y1;
            theta_offset = -gamma_rad * 180.0 / Math.PI;
            if (theta_offset <= -180.0)
                theta_offset += 360;
            else if (theta_offset > 180.0)
                theta_offset -= 360;
        }




        public double[] TopConfig_Operate(double vision_capture_x, double vision_capture_y, double vision_capture_angle_deg, double[] Calib_Data)
        {
            double transformed_vision_x, transformed_vision_y;
            double calib_theta_rad;

            // convert vision pixel to mm
            vision_capture_x /= pixel_per_mm;
            vision_capture_y /= pixel_per_mm;
            vision_capture_y = -vision_capture_y;
            // invert angle sign
            vision_capture_angle_deg = -vision_capture_angle_deg;
            calib_theta_rad = Calib_Data[2] * Math.PI / 180.0;


            //rotate vision frame first
            transformed_vision_x = vision_capture_x * Math.Cos(-calib_theta_rad) + vision_capture_y * Math.Sin(-calib_theta_rad);
            transformed_vision_y = -vision_capture_x * Math.Sin(-calib_theta_rad) + vision_capture_y * Math.Cos(-calib_theta_rad);
            //translation transformation

            Operate_Data[0] = Calib_Data[0] + transformed_vision_x;
            Operate_Data[1] = Calib_Data[1] + transformed_vision_y;
            Operate_Data[2] = Calib_Data[2] + vision_capture_angle_deg;
            if (Operate_Data[2] <= -180.0)
                Operate_Data[2] += 360;
            else if (Operate_Data[2] > 180.0)
                Operate_Data[2] -= 360;

            return Operate_Data;
        }



        public void PixelToMMConversion()
        {
            double[] pixel_distance = new double[12];

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
            pixel_per_mm = (pixel_mm[5] + pixel_mm[6]) / 2;

            for (int i = 0; i < VisionCoords.Length; i++)
            {
                VisionCoords[i].PixelConversion(pixel_per_mm);

            }
        }



        //===========================================================================================================================================
        // 2D EYE IN HAND CONFIGURATION
        //===========================================================================================================================================

        public double[] EyeInHandConfig2D_Calib(double x_move, double y_move, bool conversion)
        {
            if (conversion)
            {
                PixelToMMConversion();
            }

            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                EyeInHandConfig2D_get_XYU_Offset(VisionCoords[counter], RobotCoords[counter], VisionCoords[4], RobotCoords[4], x_move, y_move, ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
                if (i == 3) counter++;
                counter++;
            }


            // Median method
            Array.Sort(x_offset);
            Array.Sort(y_offset);
            Array.Sort(theta_offset);
            Calib_Data[0] = (x_offset[3] + x_offset[4]) / 2;
            Calib_Data[1] = (y_offset[3] + y_offset[4]) / 2;
            Calib_Data[2] = (theta_offset[3] + theta_offset[4]) / 2;

            return Calib_Data;
        }



        private static void EyeInHandConfig2D_get_XYU_Offset(Point vision_p0, Point robot_p0, Point vision_p1, Point robot_p1, double x_move, double y_move, ref double x_offset, ref double y_offset, ref double theta_offset)
        {
            {
                double alpha_rad, beta_rad, gamma_rad;
                double rotated_vision_x1, rotated_vision_y1;
                // alpha = vision deviation angle
                // beta = robot deviation angle
                // gamma = deviation angle difference

                alpha_rad = vision_p1.CalculateAtanRad(vision_p0);   //angle in vision is inverted for this configuration, thus use x0 - x1 , y0 - y1
                beta_rad = robot_p0.CalculateAtanRad(robot_p1);

                gamma_rad = alpha_rad - beta_rad;       // angle required for vision frame to align with robot frame


                //rotated vision point x1,y1 with gamma angle, so that both frame aligned, and able to find x and y offset
                rotated_vision_x1 = vision_p0.X * Math.Cos(gamma_rad) + vision_p0.Y * Math.Sin(gamma_rad);
                rotated_vision_y1 = -vision_p0.X * Math.Sin(gamma_rad) + vision_p0.Y * Math.Cos(gamma_rad);

                // offset from robot frame to vision frame
                x_offset = -robot_p0.X - x_move - rotated_vision_x1;
                y_offset = -robot_p0.Y - y_move - rotated_vision_y1;

                theta_offset = -gamma_rad * 180.0 / Math.PI;
                if (theta_offset <= -180.0)
                    theta_offset += 360;
                else if (theta_offset > 180.0)
                    theta_offset -= 360;
            }
        }

        public double[] EyeInHandConfig2D_Operate(double vision_capture_x, double vision_capture_y, double vision_capture_angle_deg, double[] Calib_Data)
        {
            double transformed_vision_x, transformed_vision_y;
            double calib_theta_rad;

            // convert vision pixel to mm
            vision_capture_x /= pixel_per_mm;
            vision_capture_y /= pixel_per_mm;
            vision_capture_y = -vision_capture_y;
            // invert angle sign
            vision_capture_angle_deg = -vision_capture_angle_deg;
            calib_theta_rad = Calib_Data[2] * Math.PI / 180.0;


            //rotate vision frame first
            transformed_vision_x = vision_capture_x * Math.Cos(-calib_theta_rad) + vision_capture_y * Math.Sin(-calib_theta_rad);
            transformed_vision_y = -vision_capture_x * Math.Sin(-calib_theta_rad) + vision_capture_y * Math.Cos(-calib_theta_rad);
            //translation transformation

            Operate_Data[0] = Calib_Data[0] + transformed_vision_x;
            Operate_Data[1] = Calib_Data[1] + transformed_vision_y;
            Operate_Data[2] = Calib_Data[2] + vision_capture_angle_deg;

            if (Operate_Data[2] <= -180.0)
                Operate_Data[2] += 360;
            else if (Operate_Data[2] > 180.0)
                Operate_Data[2] -= 360;

            return Operate_Data;
        }










        //===========================================================================================================================================
        // SCARA EYE IN HAND CONFIGURATION       (Vision mount at Arm 2)
        //===========================================================================================================================================

        public double[] EyeInHandConfigSCARA_Calib(double[] vision_x_data, double[] vision_y_data, double[] robot_joint1, double[] robot_joint2, double tcp_joint1, double tcp_joint2, double arm1_length, double arm2_length)
        {
            if (vision_x_data.Length != Vision_X.Length || vision_x_data.Length != Vision_Y.Length || robot_joint1.Length != Robot_X.Length || robot_joint2.Length != Robot_Y.Length)
            {
                // check input datapoints length
                if (vision_x_data.Length != Vision_X.Length)
                {
                    Console.WriteLine("DataPoints Vision_X_Data array are not enough!");
                    return Calib_Data;
                }
                if (vision_y_data.Length != Vision_Y.Length)
                {
                    Console.WriteLine("DataPoints Vision_Y_Data array are not enough!");
                    return Calib_Data;
                }
                if (robot_joint1.Length != Robot_X.Length)
                {
                    Console.WriteLine("DataPoints Robot_Joint_1 array are not enough!");
                    return Calib_Data;
                }
                if (robot_joint2.Length != Robot_Y.Length)
                {
                    Console.WriteLine("DataPoints Robot_Joint_2 array are not enough!");
                    return Calib_Data;
                }
            }


            // TCP point -> XY coordinate
            double x_tcp = default;
            double y_tcp = default;
            double theta_tcp = default;

            SCARA_XYMatrix(tcp_joint1, tcp_joint2, arm1_length, arm2_length, ref x_tcp, ref y_tcp, ref theta_tcp);



            // 9 points -> XY coordinate
            double[] robot_x_data = new double[9];
            double[] robot_y_data = new double[9];
            double[] total_theta = new double[9];
            /*   for (int i = 0; i < Vision_X.Length; i++)
               {
                   SCARA_XYMatrix(robot_joint1[i], robot_joint2[i], arm1_length, arm2_length, ref robot_x_data[i], ref robot_y_data[i], ref total_theta[i]);
                   Console.WriteLine("P{0} Robot X = {1}, Robot Y = {2}", i, robot_x_data[i], robot_y_data[i]);
               }*/




            // SCARA cannot do pixel per mm conversion temporary
            pixel_per_mm = 19.943048322553167; // get from scaling board

            Console.WriteLine("SCARA Pixel per mm = {0}", pixel_per_mm);


            // convert pixel to mm, and add negative to y-data
            for (int i = 0; i < vision_x_data.Length; i++)
            {
                vision_x_data[i] = vision_x_data[i] / pixel_per_mm;
                vision_y_data[i] = -vision_y_data[i] / pixel_per_mm;
            }

            Vision_X = vision_x_data;
            Vision_Y = vision_y_data;
            Robot_X = robot_x_data;
            Robot_Y = robot_y_data;

            for (int i = 0; i < 4; i++)
            {
                EyeInHandConfigSCARA_get_XYU_Offset(Vision_X[i], Vision_Y[i], Robot_X[i], Robot_Y[i], Vision_X[4], Vision_Y[4], Robot_X[4], Robot_Y[4], x_tcp, y_tcp, total_theta[i], total_theta[4], ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
            }
            for (int i = 4; i < 8; i++)
            {
                EyeInHandConfigSCARA_get_XYU_Offset(Vision_X[i + 1], Vision_Y[i + 1], Robot_X[i + 1], Robot_Y[i + 1], Vision_X[4], Vision_Y[4], Robot_X[4], Robot_Y[4], x_tcp, y_tcp, total_theta[i + 1], total_theta[4], ref x_offset[i], ref y_offset[i], ref theta_offset[i]);
            }


            // Median Method
            Array.Sort(x_offset);
            Array.Sort(y_offset);
            Array.Sort(theta_offset);
            Calib_Data[0] = (x_offset[3] + x_offset[4]) / 2;
            Calib_Data[1] = (y_offset[3] + y_offset[4]) / 2;
            Calib_Data[2] = (theta_offset[3] + theta_offset[4]) / 2;




            return Calib_Data;
        }

        private static void SCARA_XYMatrix(double joint1_angle, double joint2_angle, double arm1_length, double arm2_length, ref double X_data, ref double Y_data, ref double total_theta)
        {
            total_theta = joint1_angle + joint2_angle;

            X_data = arm2_length * Math.Cos((joint1_angle + joint2_angle) * Math.PI / 180.0) + arm1_length * Math.Cos(joint1_angle * Math.PI / 180.0);

            Y_data = arm2_length * Math.Sin((joint1_angle + joint2_angle) * Math.PI / 180.0) + arm1_length * Math.Sin(joint1_angle * Math.PI / 180.0);

        }

        private static void EyeInHandConfigSCARA_get_XYU_Offset(double vision_x1, double vision_y1, double robot_x1, double robot_y1, double vision_x0, double vision_y0, double robot_x0, double robot_y0, double x_tcp, double y_tcp, double theta1, double theta0, ref double x_offset, ref double y_offset, ref double theta_offset)
        {
            // Total 8 combinations
            // Combination 1 -> TCP -> Vision 1    =    Combination 0 -> TCP -> Vision 0
            // XM1,YM1 = from Combintaion 1 to TCP    ; XM0,YM0 = from Combintaion 0 to TCP
            double theta1_rad = theta1 * Math.PI / 180.0;
            double theta0_rad = theta0 * Math.PI / 180.0;

            double XM1 = (x_tcp - robot_x1) * Math.Cos(theta1_rad) + (y_tcp - robot_y1) * Math.Sin(theta1_rad);
            double YM1 = -(x_tcp - robot_x1) * Math.Sin(theta1_rad) + (y_tcp - robot_y1) * Math.Cos(theta1_rad);

            double XM0 = (x_tcp - robot_x0) * Math.Cos(theta0_rad) + (y_tcp - robot_y0) * Math.Sin(theta0_rad);
            double YM0 = -(x_tcp - robot_x0) * Math.Sin(theta0_rad) + (y_tcp - robot_y0) * Math.Cos(theta0_rad);

            theta_offset = Math.Atan2(((YM0 - YM1) * (vision_x0 - vision_x1) - (XM0 - XM1) * (vision_y0 - vision_y1)), ((XM0 - XM1) * (vision_x0 - vision_x1) + (YM0 - YM1) * (vision_y0 - vision_y1)));

            x_offset = XM1 - vision_x1 * Math.Cos(theta_offset) + vision_y1 * Math.Sin(theta_offset);
            y_offset = YM1 - vision_x1 * Math.Sin(theta_offset) - vision_y1 * Math.Cos(theta_offset);

            theta_offset = theta_offset * 180.0 / Math.PI;
            if (theta_offset <= -180.0)
                theta_offset += 360;
            else if (theta_offset > 180.0)
                theta_offset -= 360;

        }

        public double[] EyeInHandConfigSCARA_Operate(double vision_capture_x, double vision_capture_y, double vision_capture_angle_deg, double cur_joint1, double cur_joint2, double arm1_length, double arm2_length, double[] Calib_Data)
        {
            double calib_theta_rad;
            double cur_X = default;
            double cur_Y = default;
            double cur_total_theta = default;
            double cur_total_theta_rad;


            // convert vision pixel to mm
            vision_capture_x /= pixel_per_mm;
            vision_capture_y /= pixel_per_mm;
            vision_capture_y = -vision_capture_y;
            // invert angle sign
            vision_capture_angle_deg = -vision_capture_angle_deg;
            calib_theta_rad = Calib_Data[2] * Math.PI / 180.0;


            // calculate XY from current joint 1 and 2 
            SCARA_XYMatrix(cur_joint1, cur_joint2, arm1_length, arm2_length, ref cur_X, ref cur_Y, ref cur_total_theta);
            cur_total_theta_rad = cur_total_theta * Math.PI / 180.0;


            Operate_Data[0] = vision_capture_x * Math.Cos(cur_total_theta_rad + calib_theta_rad) - vision_capture_y * Math.Sin(cur_total_theta_rad + calib_theta_rad) + Calib_Data[0] * Math.Cos(cur_total_theta_rad) - Calib_Data[1] * Math.Sin(cur_total_theta_rad) + cur_X;
            Operate_Data[1] = vision_capture_x * Math.Sin(cur_total_theta_rad + calib_theta_rad) + vision_capture_y * Math.Cos(cur_total_theta_rad + calib_theta_rad) + Calib_Data[0] * Math.Sin(cur_total_theta_rad) + Calib_Data[1] * Math.Cos(cur_total_theta_rad) + cur_Y;
            Operate_Data[2] = cur_total_theta + Calib_Data[2] + vision_capture_angle_deg;

            if (Operate_Data[2] <= -180.0)
                Operate_Data[2] += 360;
            else if (Operate_Data[2] > 180.0)
                Operate_Data[2] -= 360;

            return Operate_Data;
        }



    }
}
