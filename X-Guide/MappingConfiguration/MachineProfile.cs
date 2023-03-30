using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class MachineProfile : Profile
    {
        public MachineProfile()
        {
            CreateMap<MachineModel, MachineViewModel>().ForMember(dest => dest.Type, opt => opt.MapFrom(src => EnumHelperClass.GetEnumDescription((MachineType)src.Type)));
            CreateMap<MachineViewModel, MachineModel>();
            CreateMap<Machine, MachineModel>();
            CreateMap<MachineModel, Machine>();
        }
      
    }
}
