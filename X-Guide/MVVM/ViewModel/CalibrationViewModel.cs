﻿namespace X_Guide.MVVM.ViewModel.CalibrationWizardSteps
{
    public class CalibrationViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public bool Mode { get; set; }
        public int Orientation { get; set; }

        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double XMove { get; set; }
        public double YMove { get; set; }
        public double JointRotationAngle { get; set; }
        public double Mm_per_pixel { get; set; }

        public double CalibratedXOffset { get; set; }
        public double CalibratedYOffset { get; set; }
        public double CalibratedRzOffset { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public double MotionDelay { get; set; }
        public string Procedure { get; set; }

        public void ResetProperties()
        {
            // Save the current value of Manipulator
            ManipulatorViewModel currentManipulator = Manipulator;

            // Reset other properties to null or default values
            Id = 0;
            Name = null;
            Manipulator = currentManipulator; // Restore the saved Manipulator value
            Mode = false;
            Orientation = 0;
            XOffset = 0.0;
            YOffset = 0.0;
            XMove = 0.0;
            YMove = 0.0;
            JointRotationAngle = 0.0;
            CalibratedXOffset = 0.0;
            CalibratedYOffset = 0.0;
            CalibratedRzOffset = 0.0;
            Speed = 0.0;
            Acceleration = 0.0;
            MotionDelay = 0.0;
            Mm_per_pixel = 0.0;
            Procedure = null;
        }
    }
}