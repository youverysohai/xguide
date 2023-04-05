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
            var mapper = new MapperConfiguration(c => { c.AddProfile<ManipulatorProfile>(); c.AddProfile<VisionProfile>(); }).CreateMapper();

            CreateMap<Calibration, CalibrationModel>();
            CreateMap<CalibrationModel, Calibration>();

            CreateMap<CalibrationModel, CalibrationViewModel>()
            .ForMember(dest => dest.Mm_per_pixel, opt => opt.MapFrom(src => src.CameraXScaling))
            .ForMember(dest => dest.Manipulator, opt => opt.MapFrom(src => mapper.Map<ManipulatorViewModel>(src.Manipulator)))
            .ForMember(dest => dest.Vision, opt => opt.MapFrom(src => mapper.Map<VisionViewModel>(src.Vision)));


            CreateMap<CalibrationViewModel, CalibrationModel>()
                .ForMember(dest => dest.CameraXScaling, opt => opt.MapFrom(src => src.Mm_per_pixel))
                    .ForMember(dest => dest.Manipulator, opt => opt.MapFrom(src => mapper.Map<ManipulatorModel>(src.Manipulator)))
            .ForMember(dest => dest.Vision, opt => opt.MapFrom(src => mapper.Map<VisionModel>(src.Vision))); ;
        }
    }


}
