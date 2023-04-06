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
        Task<CalibrationModel> Get(string name);
        Task<int> Add(CalibrationModel calibration);
        Task<int> Update(CalibrationModel calibration);

        Task<bool> IsExist(int id);
        Task<IEnumerable<CalibrationModel>> GetAll();

        Task<bool> Delete(int id);
    }
}
