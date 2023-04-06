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
    public class VisionProfile : Profile
    {
        public VisionProfile()
        {
            CreateMap<Vision, VisionModel>();
            CreateMap<VisionModel, Vision>(); 
            CreateMap<VisionModel, VisionViewModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => new ObservableCollection<string>(src.Ip.Split('.')))); ;
            CreateMap<VisionViewModel, VisionModel>().ForMember(dest => dest.Ip, opt => opt.MapFrom(src => string.Join(".", src.Ip))); ;
        }
    }
}
