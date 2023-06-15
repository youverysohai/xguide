using AutoMapper;
using X_Guide.MVVM.ViewModel;
using XGuideSQLiteDB.Models;

namespace X_Guide.MappingConfiguration
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
        }
    }
}