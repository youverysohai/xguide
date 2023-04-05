using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
    
        private CalibrationViewModel _calib;


        public CalibrationViewModel Calib
        {
            get { return _calib; }
            set
            {
                _calib = value;
                OnPropertyChanged();
            }
        }    

        public Step2ViewModel(CalibrationViewModel calib)
        {
            Calib = calib;
            
         
        
        }

    

    }
}
