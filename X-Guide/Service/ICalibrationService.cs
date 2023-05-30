using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service
{
    public interface ICalibrationService
    {
        Task<CalibrationData> Calibrate(CalibrationViewModel config);
    }
}