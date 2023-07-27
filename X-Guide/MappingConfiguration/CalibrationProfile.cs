using AutoMapper;
using CalibrationProvider;
using System.Runtime.Versioning;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB.Models;

namespace X_Guide.MappingConfiguration
{
    [SupportedOSPlatform("windows")]
    public class CalibrationProfile : Profile
    {
        public CalibrationProfile()
        {
            CreateMap<Calibration, CalibrationViewModel>().ForMember(d => d.CalibrationData, o => o.MapFrom(s => new CalibrationData
            {
                X = s.CXOffset,
                Y = s.CYOffset,
                Rz = s.CRZOffset,
                mm_per_pixel = s.MMPerPixel,
            }));
            CreateMap<CalibrationViewModel, Calibration>().ForMember(d => d.CXOffset, o => o.MapFrom(s => s.CalibrationData.X)).ForMember(d => d.CYOffset, o => o.MapFrom(s => s.CalibrationData.Y)).ForMember(d => d.CRZOffset, o => o.MapFrom(s => s.CalibrationData.Rz)).ForMember(d => d.MMPerPixel, o => o.MapFrom(s => s.CalibrationData.mm_per_pixel));
        }
    }
}