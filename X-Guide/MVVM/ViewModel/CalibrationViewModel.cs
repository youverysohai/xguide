using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.ViewModel.CalibrationWizardSteps
{
    public class CalibrationViewModel : ViewModelBase
    {
        private string _name;
        private ManipulatorViewModel _manipulator;
        private MountingOrientation _orientation;
        private string _visionFlow;
        private double _cXOffSet = 0;
        private double _cYOffset = 0;
        private double _cRZOffset = 0;
        private int _speed = 1;
        private int _acceleration = 1;
        private int _motionDelay = 0;
        private double _mm_per_pixel = 1;
        public int XOffset { get; set; } = 0;
        public int YOffset { get; set; } = 0;

        public double Mm_per_pixel
        {
            get { return _mm_per_pixel; }
            set
            {
                _mm_per_pixel = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged();
            }

        }

        public double CXOffSet
        {
            get { return _cXOffSet; }
            set
            {
                _cXOffSet = value;
                OnPropertyChanged();
            }
        }
        public double CYOffset
        {
            get { return _cYOffset; }
            set
            {
                _cYOffset = value;
                OnPropertyChanged();
            }
        }

        public double CRZOffset
        {
            get { return _cRZOffset; }
            set { _cRZOffset = value;
                OnPropertyChanged();
            }
        }
        public ManipulatorViewModel Manipulator
        {
            get => _manipulator; set
            {
                _manipulator = value;
                OnPropertyChanged();
            }
        }
        public MountingOrientation Orientation
        {
            get => _orientation; set
            {
                _orientation = value;
                OnPropertyChanged();
            }
        }
        public string VisionFlow
        {
            get => _visionFlow; set
            {
                _visionFlow = value;
                OnPropertyChanged();
            }
        }

        public int Speed
        {
            get => _speed; set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }

        public int Acceleration
        {
            get => _acceleration; set
            {
                _acceleration = value;
                OnPropertyChanged();
            }
        }

        public int MotionDelay
        {
            get => _motionDelay; set
            {
                _motionDelay = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\nManipulator: {1}\nOrientation: {2}\nVision Flow: {3}\nSpeed: {4}\nAcceleration: {5}\nMotion Delay: {6}",
    _name, _manipulator, _orientation, _visionFlow, _speed, _acceleration, _motionDelay);


        }
    }
}
