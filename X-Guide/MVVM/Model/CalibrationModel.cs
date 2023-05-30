namespace X_Guide.MVVM.Model
{
    public class CalibrationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManipulatorId { get; set; }
        public int Orientation { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public double MotionDelay { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double XMove { get; set; }
        public double YMove { get; set; }
        public double JointRotationAngle { get; set; }
        public double CameraXScaling { get; set; }
        public double CameraYScaling { get; set; }
        public double CalibratedXOffset { get; set; }
        public double CalibratedYOffset { get; set; }
        public double CalibratedRzOffset { get; set; }
        public ManipulatorModel Manipulator { get; set; }

        public string Procedure { get; set; }
        public bool Mode { get; set; }
    }
}