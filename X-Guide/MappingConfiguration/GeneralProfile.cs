using AutoMapper;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<GeneralConfiguration, GeneralModel>().ReverseMap();
            CreateMap<GeneralViewModel, GeneralModel>().ReverseMap();
        }
    }
}