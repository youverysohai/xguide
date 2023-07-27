using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{
    internal class CalibrationRepository : Repository<Calibration>, ICalibrationRepository
    {
        public Task<Calibration> GetWithManipulatorByIdAsync(int id)
        {
            using (var db = new XGuideDbContext())
            {
                return db.Calibrations.Include(e => e.Manipulator).SingleAsync(e => e.Equals(id));
            }
        }
    }
}