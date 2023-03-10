using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {
        public override ViewModelBase GetNextViewModel()
        {
            return new Step5ViewModel();
        }
        private Duration _duration;

        public Duration Duration
        {
            get { return _duration; }
            set { _duration = value;
                OnPropertyChanged();
            }
        }
        public Storyboard Storyboard { get; set; }

        private int _angle;

        public int Angle
        {
            get { return _angle; }
            private set
            {
                _angle = value;
                OnPropertyChanged();
            }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (value > 0 && value < 170)
                {
                    _value = value;
                    Angle = (int)(value * 1.7 - 85);
                    OnPropertyChanged();
                }

            }
        }

        private int _angleAcceleration;

        public int AngleAcceleration
        {
            get { return _angleAcceleration; }
            private set
            {
                _angleAcceleration = value;
                OnPropertyChanged();
            }
        }

        private int _valueAcceleration;

        public int ValueAcceleration
        {
            get { return _valueAcceleration; }
            set
            {
                if (value > 0 && value < 170)
                {
                    _valueAcceleration = value;
                    AngleAcceleration = (int)(value * 1.7 - 85);
                    OnPropertyChanged();
                }

            }
        }
        public Step4ViewModel()
        {
          
        }


    }
}
