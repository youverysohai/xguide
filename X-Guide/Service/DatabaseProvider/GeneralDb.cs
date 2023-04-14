using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service.DatabaseProvider
{
    internal class GeneralDb : DbServiceBase, IGeneralDb
    {
        private readonly Configuration _configuration;

        public GeneralDb(IMapper mapper, Configuration configuration) : base(null, mapper)
        {
            _configuration = configuration;
        }
        public GeneralViewModel Get()
        {
            return MapTo<GeneralViewModel>((General)_configuration.GetSection("GeneralSetting"));
     
        }

        public void Update(GeneralViewModel generalVm)
        {
            var general = (General)_configuration.GetSection("GeneralSetting");
            Map(generalVm, general);
            _configuration.Save();
        }
    }
}
