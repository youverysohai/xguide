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
        public double JointRotationAngle { get; set; }
        public double CameraXScaling { get; set; }
        public double CameraYScaling { get; set; }
        public double CRZOffset { get; set; }
        public double CYOffset { get; set; }
        public double CXOffset { get; set; }
        public ManipulatorModel Manipulator { get; set; }

        public string Procedure { get; set; }
        public bool JogMode { get; set; }
        public bool CalibrationMode { get; set; }
    }
}