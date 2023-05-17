using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class HikVisionDb : DbServiceBase, IVisionDb
    {
        private readonly Configuration _configuration;
        private readonly HikVisionConfiguration _visionConfiguration;

        public HikVisionDb(IMapper mapper, Configuration configuration) : base(null, mapper)
        {
            _configuration = configuration;
            _visionConfiguration = (HikVisionConfiguration)configuration.GetSection("HikVisionSetting");
        }

        public Task<bool> Add(HikVisionModel vision)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(HikVisionModel vision)
        {
            throw new NotImplementedException();
        }

        public HikVisionModel Get()
        {
            return MapTo<HikVisionModel>(_visionConfiguration);
        }

        public Task<IEnumerable<HikVisionModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(HikVisionModel vision)
        {
            Map(vision, _visionConfiguration);
            _configuration.Save();
        }
    }
}