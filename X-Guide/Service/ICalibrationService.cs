using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service
{
    public interface ICalibrationService
    {
        void EyeInHand2DConfig_Calibrate(CalibrationViewModel calibration);
    }
}
