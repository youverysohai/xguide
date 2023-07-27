using System.Threading.Tasks;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{
    internal interface ICalibrationRepository : IRepository<Calibration>
    {
        Task<Calibration> GetWithManipulatorByIdAsync(int id);
    }
}