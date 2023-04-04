using Autofac;
using Autofac.Core;
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
        private readonly IViewModelLocator _viewModelLocator;

        public NavigationService(NavigationStore navigationStore, IViewModelLocator viewModelLocator)
        {
            _navigationStore = navigationStore;
            _viewModelLocator = viewModelLocator;
        }
       
        public void Navigate(ViewModelBase viewModelBase)
        {
            _navigationStore.CurrentViewModel = viewModelBase;
        }
        public ViewModelBase Navigate<T>(params Parameter[] parameters) where T : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelLocator.Create<T>(parameters);
            _navigationStore.CurrentViewModel = viewModel;
            return viewModel;
        }

        public NavigationStore GetNavigationStore()
        {
            return _navigationStore;
        }
        
    }
}
