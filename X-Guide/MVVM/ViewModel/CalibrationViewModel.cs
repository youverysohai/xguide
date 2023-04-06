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



        private int _id;
        private string _name;
        private ManipulatorViewModel _manipulator;
        private VisionViewModel _vision;
        private int _orientation;
        private double _cXOffSet = 0;
        private double _cYOffset = 0;
        private double _cRZOffset = 0;
        private double _speed = 1;
        private double _acceleration = 1;
        private double _motionDelay = 0;
        private double _mm_per_pixel = 1;
        private string _procedure;

        public string Procedure
        {
            get { return _procedure; }
            set { _procedure = value; }
        }



        public int XOffset { get; set; } = 0;
        public int YOffset { get; set; } = 0;


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

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
            get => _manipulator; 
            set
            {
                _manipulator = value;
                OnPropertyChanged();
            }
        }

        

        public VisionViewModel Vision
        {
            get { return _vision; }
            set { _vision = value; }
        }


        public int Orientation
        {
            get => _orientation; 
            set
            {
                _orientation = value;
                OnPropertyChanged();
            }
        }


        public double Speed
        {
            get => _speed; set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }

        public double Acceleration
        {
            get => _acceleration; set
            {
                _acceleration = value;
                OnPropertyChanged();
            }
        }

        public double MotionDelay
        {
            get => _motionDelay; set
            {
                _motionDelay = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\nManipulatorId: {1}\nOrientation: {2}\nVision Flow: {3}\nSpeed: {4}\nAcceleration: {5}\nMotion Delay: {6}",
    _name, _manipulator, _orientation, _procedure, _speed, _acceleration, _motionDelay);


        }
    }
}
