using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        {/*.ForMember(dest => dest.Type, opt => opt.MapFrom(src => EnumHelperClass.GetEnumDescription((MachineType)src.Type)))*/
            CreateMap<MachineModel, MachineViewModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => new ObservableCollection<string>(src.Ip.Split('.'))));
            CreateMap<MachineViewModel, MachineModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => string.Join(".", src.Ip)));
            CreateMap<Machine, MachineModel>();
            CreateMap<MachineModel, Machine>();
        }
      
    }
}
