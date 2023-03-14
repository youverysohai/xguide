using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {

        private CalibrationViewModel _setting;

        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set { _setting = value;
                OnPropertyChanged();
            }
        }



        public override ViewModelBase GetNextViewModel()
        {
            
            return new Step5ViewModel();
        }
        
    
        public Step4ViewModel(CalibrationViewModel setting)
        {
            _setting = setting; 
        }


    }
}
