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
    public class ManipulatorProfile : Profile
    {
        public ManipulatorProfile()
        {/*.ForMember(dest => dest.Type, opt => opt.MapFrom(src => EnumHelperClass.GetEnumDescription((MachineType)src.Type)))*/
            CreateMap<ManipulatorModel, ManipulatorViewModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => new ObservableCollection<string>(src.Ip.Split('.'))));
            CreateMap<ManipulatorViewModel, ManipulatorModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => string.Join(".", src.Ip)));
            CreateMap<Manipulator, ManipulatorModel>();
            CreateMap<ManipulatorModel, Manipulator>();
        }
      
    }
}
