using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class VisionProfile : Profile
    {
        public VisionProfile()
        {
            CreateMap<Vision, VisionViewModel>();
            CreateMap<VisionViewModel, Vision>();

        }
    }
}
