using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service.DatabaseProvider
{
    internal class CalibrationDb : DbServiceBase, ICalibrationDb
    {

        public CalibrationDb(DbContextFactory dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {
       
        }

        public async Task<CalibrationModel> GetCalibration(string name)
        {
            return await AsyncQuery((context) =>
            {
                return MapTo<CalibrationModel>(context.Calibrations.FirstOrDefault(x => x.Name == name)) ;
            });
        }

        public async Task<IEnumerable<CalibrationModel>> GetCalibrations()
        {
            return await AsyncQuery((context) =>
            {
              return context.Calibrations.Include("Manipulator").ToList().Select(x=> MapTo<CalibrationModel>(x));
               
            });
        }

        public async Task<int> AddCalibration(CalibrationModel calibration)
        {
            return await AsyncQuery((context) =>
            {
                context.Calibrations.Add(MapTo<Calibration>(calibration));
                return context.SaveChanges();

             
            });
        }

        public async Task<bool> DeleteCalibration(int id)
        {
            return await AsyncQuery((context) =>
            {
                try
                {

                    Calibration calibration = context.Calibrations.Find(id);
                    if (calibration == null) throw new Exception("Data not found!");
                    context.Calibrations.Remove(calibration);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }
            });
        }
    }
}
