using CalibrationTest;
using System;
using System.Threading.Tasks;
using VisionGuided;
using X_Guide;

namespace CalibrationTesting
{
    internal class Program
    {
        private static readonly _9PointCalibration _9PointCalibration = new _9PointCalibration();

        private static async Task Main(string[] args)
        {
            await PrintPoints();
        }

        private static async Task PrintPoints()
        {
            var i = await _9PointCalibration.Start9PointCalib();

            Console.WriteLine();
            PrintAllPoints(i.Item1);
            Console.ReadLine();
        }

        private static void PrintAllPoints(Point[] robots)
        {
            for (int i = 0; i < robots.Length; i++)
            {
                Console.WriteLine($"Robot point {i} : {robots[i]}");
            }
            Console.WriteLine("\n\n\n");
        }
    }
}