using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service.DatabaseProvider
{
    public interface ICalibrationService
    {
        Task<CalibrationViewModel> GetCalibration(string name);
        Task<int> AddCalibration(CalibrationViewModel calibration);
    }
}
