using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
        private MachineViewModel _machine;

        public MachineViewModel Machine
        {
            get { return _machine; }
            set { _machine = value;
                OnPropertyChanged();
            }
        }
        public event EventHandler OnSelectedChangedEvent;

        public Step2ViewModel(MachineViewModel machine, ref EventHandler OnSelectedItemChangedEvent)
        {
            Machine = machine;
           OnSelectedItemChangedEvent += OnSelectedChangedEventHandler;

        }
        
        private void OnSelectedChangedEventHandler(object sender, EventArgs e)
        {
            Machine = ((dynamic)sender).Machine;
            
        }

        

        public override ViewModelBase GetNextViewModel()
        {
            return new Step3ViewModel();
        }
        

    }
}
