using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
        public override ViewModelBase GetNextViewModel()
        {
            return new Step3ViewModel();
        }
    }
}
