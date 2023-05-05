using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class VisionDb : DbServiceBase, IVisionDb
    {
        public VisionDb(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory, mapper)
        {
        }

        public async Task<bool> Delete(VisionModel vision)
        {
            return await AsyncQuery(c =>
            {
                var result = c.Visions.Find(vision.Id);
                if (result == null) return false;
                List<Calibration> calibrations = c.Calibrations.Where(x => x.VisionId == result.Id).ToList();
                calibrations.ForEach(x => x.Vision = null);
                c.Visions.Remove(result);
                c.SaveChanges();
                return true;
            });
        }

        public async Task<bool> Update(VisionModel vision)
        {
            return await AsyncQuery(c =>
            {
                var result = c.Visions.Find(vision.Id);
                if (result == null) return false;
                c.Entry(result).CurrentValues.SetValues(MapTo<Vision>(vision));
                c.SaveChanges();
                return true;
            });
        }

        public async Task<VisionModel> Get(string name)
        {
            return await AsyncQuery(c =>
            {
                return MapTo<VisionModel>(c.Visions.FirstOrDefault(x => x.Name.Equals(name)));
            });
        }

        public async Task<IEnumerable<VisionModel>> GetAll()
        {
            return await AsyncQuery(c =>
            {
                return c.Visions.ToList().Select(x => MapTo<VisionModel>(x));
            });
        }

        public async Task<bool> Add(VisionModel vision)
        {
            return await AsyncQuery(c =>
            {
                c.Visions.Add(MapTo<Vision>(vision));
                c.SaveChanges();
                return true;
            });
        }
    }
}