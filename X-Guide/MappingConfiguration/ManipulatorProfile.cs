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
        {

            CreateMap<ManipulatorModel, ManipulatorViewModel>();
       
            CreateMap<ManipulatorViewModel, ManipulatorModel>();

            CreateMap<Manipulator, ManipulatorModel>();
            CreateMap<ManipulatorModel, Manipulator>();
        }
      
    }
}
