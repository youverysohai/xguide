using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service
{
    public class NavigationService : INavigationService
    {
        private NavigationStore _navigationStore;
        
        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
       
        public void Navigate(ViewModelBase viewModel)
        {
            _navigationStore.CurrentViewModel = viewModel;
        }
    
    }
}
