﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }
        public void Navigate<T>() where T : ViewModelBase
        {
            _navigationStore.CurrentViewModel = Activator.CreateInstance<T>();
        }
        public void Navigate(ViewModelBase viewModel)
        {
            _navigationStore.CurrentViewModel = viewModel;
        }
        public void Navigate() 
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
