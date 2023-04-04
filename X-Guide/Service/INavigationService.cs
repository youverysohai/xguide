using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service
{
    public interface INavigationService
    {
        void Navigate(ViewModelBase viewModelBase);
        ViewModelBase Navigate<T>(params Parameter[] parameters) where T : ViewModelBase;
        NavigationStore GetNavigationStore();

    }
}
