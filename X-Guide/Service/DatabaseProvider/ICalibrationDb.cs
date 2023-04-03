using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service.DatabaseProvider
{
    public interface ICalibrationDb
    {
        Task<CalibrationViewModel> GetCalibration(string name);
        Task<int> AddCalibration(CalibrationViewModel calibration);
        Task<IEnumerable<Calibration>> GetAllCalibration();
    }
}
