using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class GeneralProfile : Profile
    {

        public GeneralProfile()
        {
            CreateMap<General, GeneralViewModel>();
            CreateMap<GeneralViewModel, General>();
        }
    }
}
