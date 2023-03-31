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

        public async Task<VisionViewModel> GetVision(string name)
        {
            return await AsyncQuery((context) =>
            {
                return MapTo<VisionViewModel>(context.Visions.FirstOrDefault(x => x.Name.Equals(name)));
            });
        }
    }
}
