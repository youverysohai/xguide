using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service.DatabaseProvider
{
    public interface ICalibrationDb
    {
        Task<CalibrationModel> GetCalibration(string name);
        Task<int> AddCalibration(CalibrationModel calibration);
        Task<IEnumerable<CalibrationModel>> GetCalibrations();

        Task<bool> DeleteCalibration(int id);
    }
}
