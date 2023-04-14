using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using X_Guide.Communication.Service;
using X_Guide.Enums;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {

        private readonly CalibrationViewModel _calibration;
        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        public readonly IEnumerable<ValueDescription> _orientationType = EnumHelperClass.GetAllIntAndDescriptions(typeof(Orientation));
        public IEnumerable<ValueDescription> OrientationType
        {
            get { return _orientationType; }
        }


        public RelayCommand OrientationCommand { get; set; }
        public Step2ViewModel(CalibrationViewModel calibration)
        {
            _calibration = calibration;
        }

    }
}
