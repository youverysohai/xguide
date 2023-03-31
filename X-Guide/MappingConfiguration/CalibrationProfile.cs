using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

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
