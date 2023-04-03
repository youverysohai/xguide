using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service.DatabaseProvider
{
    internal class CalibrationDb : DbServiceBase, ICalibrationDb
    {

        public CalibrationDb(DbContextFactory dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {
       
        }

        public async Task<CalibrationViewModel> GetCalibration(string name)
        {
            return await AsyncQuery((context) =>
            {
                return MapTo<CalibrationViewModel>(context.Calibrations.FirstOrDefault(x => x.Name == name)) ;
            });
        }

        public async Task<IEnumerable<Calibration>> GetAllCalibration()
        {
            return await AsyncQuery((context) =>
            {
                return context.Calibrations.ToList();
               
            });
        }

        public async Task<int> AddCalibration(CalibrationViewModel calibration)
        {
            return await AsyncQuery((context) =>
            {
                context.Calibrations.Add(MapTo<Calibration>(calibration));
                return context.SaveChanges();

             
            });
        }
    }
}
