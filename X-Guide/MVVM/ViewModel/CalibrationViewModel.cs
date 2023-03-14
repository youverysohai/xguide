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
        private MachineViewModel _machine;
        private MountingOrientation _orientation;
        private string _visionFlow;
        private int _speed;
        private int _acceleration;
        private int _motionDelay;

        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged();
            }

        }
        public MachineViewModel Machine
        {
            get => _machine; set
            {
                _machine = value;
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
            return string.Format("Name: {0}\nMachine: {1}\nOrientation: {2}\nVision Flow: {3}\nSpeed: {4}\nAcceleration: {5}\nMotion Delay: {6}",
    _name, _machine, _orientation, _visionFlow, _speed, _acceleration, _motionDelay);


        }
    }
}
