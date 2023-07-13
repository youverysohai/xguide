using CalibrationProvider;
using X_Guide.Enums;
using Point = VisionGuided.Point;

namespace X_Guide.MVVM.ViewModel.CalibrationWizardSteps
{
    public class CalibrationViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public bool JogMode { get; set; }
        public bool CalibrationMode { get; set; }
        public Orientation Orientation { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public double JointRotationAngle { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public int MotionDelay { get; set; }
        public string Procedure { get; set; }
        public CalibrationData CalibrationData { get; set; }

        public Point[] VisionPoints { get; set; }
        public Point[] RobotPoints { get; set; }

        public int XMove { get; set; }
        public int YMove { get; set; }

        public void ResetProperties()
        {
            // Save the current value of Manipulator
            ManipulatorViewModel currentManipulator = Manipulator;

            // Reset other properties to null or default values
            Id = 0;
            Name = null;
            Manipulator = currentManipulator; // Restore the saved Manipulator value
            JogMode = false;
            CalibrationMode = false;
            Orientation = Orientation.EyeOnHand;
            XOffset = 0;
            YOffset = 0;
            JointRotationAngle = 0.0;
            Speed = 0.0;
            Acceleration = 0.0;
            MotionDelay = 0;
            Procedure = null;
            XMove = 0;
            YMove = 0;
        }
    }
}