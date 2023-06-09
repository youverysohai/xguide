using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMControls.Winform.Release;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    internal interface IVisionCalibrationStep
    {
        void SetConfig(CalibrationViewModel config);
    }
}
