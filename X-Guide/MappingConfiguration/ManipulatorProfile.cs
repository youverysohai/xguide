using AutoMapper;
using X_Guide.MVVM.ViewModel;
using XGuideSQLiteDB.Models;

namespace X_Guide.MappingConfiguration
{
    public class ManipulatorProfile : Profile
    {
        public ManipulatorProfile()
        {
            CreateMap<Manipulator, ManipulatorViewModel>().ReverseMap();
        }
    }
}