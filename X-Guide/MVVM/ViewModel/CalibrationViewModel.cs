namespace X_Guide.MVVM.ViewModel.CalibrationWizardSteps
{
    public class CalibrationViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public bool JogMode { get; set; }
        public bool CalibrationMode { get; set; }
        public int Orientation { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public double JointRotationAngle { get; set; }
        public double CXOffset { get; set; }
        public double CYOffset { get; set; }
        public double CRZOffset { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public int MotionDelay { get; set; }
        public double Mm_per_pixel { get; set; }
        public string Procedure { get; set; }

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
        Orientation = 0;
        XOffset = 0;
        YOffset = 0;
        JointRotationAngle = 0.0;
        CXOffset = 0.0;
        CYOffset = 0.0;
        CRZOffset = 0.0;
        Speed = 0.0;
        Acceleration = 0.0;
        MotionDelay = 0;
        Mm_per_pixel = 0.0;
        Procedure = null;
    }
    }
}