using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service.DatabaseProvider
{
    internal class VisionDb : DbServiceBase, IVisionDb
    {


        public VisionDb(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory, mapper)
        {

        }

        public async Task<bool> RemoveVision(string name)
        {
            return await AsyncQuery(c =>
            {
                if(c.Visions.Remove(c.Visions.FirstOrDefault(x => x.Name == name)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            });
        }

        public async Task<bool> UpdateVision(VisionViewModel vision)
        {
            
            return await AsyncQuery(c => {
                var result = c.Visions.Where(x => x.Name.Equals(vision.Name));
                if (result == null) return false;
                c.Entry(result).CurrentValues.SetValues(MapTo<Vision>(vision));
                c.SaveChanges();
                return true;
            });
        }

        public async Task<VisionViewModel> GetVision(string name)
        {
            return await AsyncQuery(c =>
            {
                return MapTo<VisionViewModel>(c.Visions.FirstOrDefault(x => x.Name.Equals(name)));
            });
        }


        public async Task<IEnumerable<VisionViewModel>> GetAllVision()
        {
            return await AsyncQuery(c =>
            {
                return c.Visions.ToList().Select(x => MapTo<VisionViewModel>(x));
            });
        }

    }
}
