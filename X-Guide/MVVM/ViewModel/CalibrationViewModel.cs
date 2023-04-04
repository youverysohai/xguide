﻿using System;
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
        private Manipulator _manipulator;
        private int _orientation;
        private string _visionFlow;
        private double _cXOffSet = 0;
        private double _cYOffset = 0;
        private double _cRZOffset = 0;
        private double _speed = 1;
        private double _acceleration = 1;
        private double _motionDelay = 0;
        private double _mm_per_pixel = 1;
        public int XOffset { get; set; } = 0;
        public int YOffset { get; set; } = 0;

        private string _visionFilePath;

        public string VisionFilePath
        {
            get { return _visionFilePath; }
            set { _visionFilePath = value;
                OnPropertyChanged();
            }
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
        public Manipulator Manipulator
        {
            get => _manipulator; 
            set
            {
                _manipulator = value;
                OnPropertyChanged();
            }
        }
        public int Orientation
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
    _name, _manipulator, _orientation, _visionFlow, _speed, _acceleration, _motionDelay);


        }
    }
}
