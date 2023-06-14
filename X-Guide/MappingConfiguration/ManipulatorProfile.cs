using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Enums;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class ManipulatorProfile : Profile
    {
        public ManipulatorProfile()
        {

            CreateMap<ManipulatorModel, ManipulatorViewModel>();

            CreateMap<ManipulatorViewModel, ManipulatorModel>();

            CreateMap<Manipulator, ManipulatorModel>().ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(ManipulatorType), src.Type)));
            CreateMap<ManipulatorModel, Manipulator>().ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));
        }
    }
}
