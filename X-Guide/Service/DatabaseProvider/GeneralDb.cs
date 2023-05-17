using AutoMapper;
using System.Configuration;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class GeneralDb : DbServiceBase, IGeneralDb
    {
        private readonly Configuration _configuration;

        public GeneralDb(IMapper mapper, Configuration configuration) : base(null, mapper)
        {
            _configuration = configuration;
        }

        public GeneralModel Get()
        {
            return MapTo<GeneralModel>((GeneralConfiguration)_configuration.GetSection("GeneralSetting"));
        }

        public void Update(GeneralModel generalVm)
        {
            var general = (GeneralConfiguration)_configuration.GetSection("GeneralSetting");
            Map(generalVm, general);
            _configuration.Save();
        }
    }
}