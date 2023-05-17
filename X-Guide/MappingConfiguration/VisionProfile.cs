using AutoMapper;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MappingConfiguration
{
    public class VisionProfile : Profile
    {
        public VisionProfile()
        {
            CreateMap<HikVisionConfiguration, HikVisionModel>();
            CreateMap<HikVisionModel, HikVisionConfiguration>();
            CreateMap<HikVisionModel, HikVisionViewModel>();
            CreateMap<HikVisionViewModel, HikVisionModel>();
        }
    }
}