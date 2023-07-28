using CalibrationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class ManualNinePointCalibrationViewModel : ViewModelBase
    {
        public Provider provider { get; set; } = Provider.Manipulator;

        public ManualNinePointCalibrationViewModel()
        {
                
        }
    }
}
