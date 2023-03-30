using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;

namespace X_Guide.Service.DatabaseProvider
{
    internal class VisionDbService : DbServiceBase, IVisionDbService
    {
        private readonly IMapper _mapper;

        public VisionDbService(IMapper mapper, DbContextFactory contextFactory) : base(contextFactory)
        {
            _mapper = mapper;
        }

        
    }
}
