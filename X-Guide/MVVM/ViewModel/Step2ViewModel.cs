using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
        private MachineModel machine;

        public Step2ViewModel(MachineModel machine)
        {
            this.machine = machine;
        }

        public override ViewModelBase GetNextViewModel()
        {
            return new Step3ViewModel();
        }
        

    }
}
