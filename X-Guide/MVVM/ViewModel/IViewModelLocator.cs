using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    public interface IViewModelLocator
    {
        ViewModelBase Create<T>() where T : ViewModelBase;

        ViewModelBase CreateCalibrationWizardStart(NavigationStore navigationStore);
        ViewModelBase CreateCalibrationMainViewModel(string name);
        ViewModelBase CreateStep1(CalibrationViewModel setting);
        ViewModelBase CreateStep2(CalibrationViewModel setting);
    }
}
