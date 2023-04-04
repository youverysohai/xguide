using Autofac.Core;
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
        ViewModelBase Create<T>(params Parameter[] parameters) where T : ViewModelBase;
  
    }
}
