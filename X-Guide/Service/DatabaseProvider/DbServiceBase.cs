using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;

namespace X_Guide.Service.DatabaseProvider
{
    internal class DbServiceBase
    {
        private readonly DbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public DbServiceBase(DbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }
        protected async Task<T> AsyncQuery<T>(Func<XGuideDBEntities, T> action)
        {
            return await Task.Run(() =>
            {
                using (XGuideDBEntities context = _dbContextFactory.CreateDbContext())
                {
                    return action(context);
                }
            });
        }

        protected T MapTo<T>(object destination)
        {
            return _mapper.Map<T>(destination);
        }

       


    }
}
