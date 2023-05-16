using AutoMapper;
using X_Guide.MVVM.DBContext;

namespace X_Guide.Service.DatabaseProvider
{
    internal class LegacyVisionDb : DbServiceBase
    {
        public LegacyVisionDb(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory, mapper)
        {
        }

        //public async Task<bool> Delete(HikVisionModel vision)
        //{
        //    return await AsyncQuery(c =>
        //    {
        //        var result = c.Visions.Find(vision.Id);
        //        if (result == null) return false;
        //        List<Calibration> calibrations = c.Calibrations.Where(x => x.VisionId == result.Id).ToList();
        //        calibrations.ForEach(x => x.Vision = null);
        //        c.Visions.Remove(result);
        //        c.SaveChanges();
        //        return true;
        //    });
        //}

        //public async Task<bool> Update(HikVisionModel vision)
        //{
        //    return await AsyncQuery(c =>
        //    {
        //        var result = c.Visions.Find(vision.Id);
        //        if (result == null) return false;
        //        c.Entry(result).CurrentValues.SetValues(MapTo<Vision>(vision));
        //        c.SaveChanges();
        //        return true;
        //    });
        //}

        //public async Task<HikVisionModel> Get(string name)
        //{
        //    return await AsyncQuery(c =>
        //    {
        //        return MapTo<HikVisionModel>(c.Visions.First());
        //        //return MapTo<VisionModel>(c.Visions.FirstOrDefault(x => x.Name.Equals(name)));
        //    });
        //}

        //public async Task<IEnumerable<HikVisionModel>> GetAll()
        //{
        //    return await AsyncQuery(c =>
        //    {
        //        return c.Visions.ToList().Select(x => MapTo<HikVisionModel>(x));
        //    });
        //}

        //public async Task<bool> Add(HikVisionModel vision)
        //{
        //    return await AsyncQuery(c =>
        //    {
        //        c.Visions.Add(MapTo<Vision>(vision));
        //        c.SaveChanges();
        //        return true;
        //    });
        //}

        //public HikVisionModel Get()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}