using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MappingConfiguration
{
    public class CalibrationProfile : Profile
    {

        public CalibrationProfile()
        {
            var maniConfig = new MapperConfiguration(c => c.AddProfile<ManipulatorProfile>()).CreateMapper();

            CreateMap<Calibration, CalibrationModel>();
            CreateMap<CalibrationModel, Calibration>();

            CreateMap<CalibrationModel, CalibrationViewModel>()
            .ForMember(dest => dest.Mm_per_pixel, opt => opt.MapFrom(src => src.CameraXScaling))
            .ForMember(dest => dest.Manipulator, opt => opt.MapFrom(src => maniConfig.Map<ManipulatorViewModel>(maniConfig.Map<ManipulatorModel>(src.Manipulator))));

            CreateMap<CalibrationViewModel, CalibrationModel>()
                .ForMember(dest => dest.CameraXScaling, opt => opt.MapFrom(src => src.Mm_per_pixel))
                .ForMember(dest => dest.CameraYScaling, opt => opt.MapFrom(src => src.Mm_per_pixel))
                .ForMember(dest => dest.Manipulator, opt => opt.MapFrom(src => maniConfig.Map<ManipulatorModel>(maniConfig.Map<Manipulator>(src.Manipulator))));
        }
    }


}
