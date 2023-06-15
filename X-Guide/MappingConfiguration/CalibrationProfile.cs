using AutoMapper;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB.Models;

namespace X_Guide.MappingConfiguration
{
    public class CalibrationProfile : Profile
    {
        public CalibrationProfile()
        {
            CreateMap<Calibration, CalibrationViewModel>();
            CreateMap<CalibrationViewModel, Calibration>();
        }
    }
}