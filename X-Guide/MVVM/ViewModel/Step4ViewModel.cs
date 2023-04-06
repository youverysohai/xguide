using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using X_Guide.Communication.Service;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {
        

        private readonly CalibrationViewModel _calibration;


        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

      

   
    
        public Step4ViewModel(CalibrationViewModel calibration)
        {
            _calibration = calibration;
        }


    }
}
