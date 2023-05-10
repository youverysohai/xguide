using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class CalibrationDb : DbServiceBase, ICalibrationDb
    {
        public CalibrationDb(DbContextFactory dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {
        }

        public async Task<CalibrationModel> Get(string name)
        {
            return await AsyncQuery((context) =>
            {
                return MapTo<CalibrationModel>(context.Calibrations.FirstOrDefault(x => x.Name == name));
            });
        }

        public async Task<IEnumerable<CalibrationModel>> GetAll()
        {
            return await AsyncQuery((context) =>
            {
                return context.Calibrations.Include("Manipulator").Include("Vision").ToList().Select(x => MapTo<CalibrationModel>(x));
            });
        }

        public async Task<int> Update(CalibrationModel calibration)
        {
            return await AsyncQuery((context) =>
            {
                Calibration calib = context.Calibrations.Find(calibration.Id);
                context.Entry(calib).CurrentValues.SetValues(MapTo<Calibration>(calibration));
                return context.SaveChanges();
            });
        }

        public async Task<int> Add(CalibrationModel calibration)
        {
            return await AsyncQuery((context) =>
            {
                Calibration calib = MapTo<Calibration>(calibration);
                calib.Manipulator = context.Manipulators.Find(calib.Manipulator.Id);
                //calib.Vision = context.Visions.Find(calib.Vision.Id);
                context.Calibrations.Add(calib);
                return context.SaveChanges();
            });
        }

        public async Task<bool> Delete(int id)
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
                    return false;
                }
            });
        }

        public async Task<bool> IsExist(int id)
        {
            return await AsyncQuery((context) =>
            {
                return context.Calibrations.Find(id) is null ? false : true;
            });
        }
    }
}