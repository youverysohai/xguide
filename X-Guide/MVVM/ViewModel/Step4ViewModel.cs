using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step4ViewModel : ViewModelBase
    {
        public override ViewModelBase GetNextViewModel()
        {
            return new Step5ViewModel();
        }
    }
}
