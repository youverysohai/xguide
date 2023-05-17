using System;
using System.Collections.Generic;
using System.Linq;
using X_Guide.Enums;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase, ICalibrationStep
    {
        private readonly CalibrationViewModel _calibration;

        public CalibrationViewModel Calibration
        {
            get
            {
                if (_calibration.Orientation == default) OnStateChanged?.Invoke(false);
                else OnStateChanged?.Invoke(true);
                return _calibration;
            }
        }

        public List<ValueDescription> _orientationType = EnumHelperClass.GetAllIntAndDescriptions(typeof(Orientation)).ToList();

        public IEnumerable<ValueDescription> OrientationType
        {
            get { return _orientationType; }
        }

        public RelayCommand OrientationCommand { get; set; }
        public Action<bool> OnStateChanged { get; private set; }

        public Step2ViewModel(CalibrationViewModel calibration)
        {
            _calibration = calibration;
            switch (calibration.Manipulator.Type)
            {
                case (int)ManipulatorType.GantrySystemR:
                case (int)ManipulatorType.GantrySystemWR: _orientationType = _orientationType.GetRange(0, 3); break;
                case (int)ManipulatorType.SCARA: _orientationType.RemoveAt(4); break;
                case (int)ManipulatorType.SixAxis: _orientationType.RemoveAt(3); break;
            }
        }

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }
    }
}