using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
    
        private CalibrationViewModel _setting;

        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
                OnPropertyChanged();
            }
        }


        public Step2ViewModel(ref EventHandler OnSelectedItemChangedEvent, CalibrationViewModel setting)
        {
            Setting = setting;
            OnSelectedItemChangedEvent += OnSelectedChangedEventHandler;
        }

        private void OnSelectedChangedEventHandler(object sender, EventArgs e)
        {
            Setting.Machine = ((dynamic)sender).Machine;
        }



        public override ViewModelBase GetNextViewModel()
        {
            return new Step3ViewModel(_setting);
        }


    }
}
