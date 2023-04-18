using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using X_Guide.MVVM;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.State;

namespace X_Guide.Service
{
    public class NavigationService : INavigationService
    {
        private NavigationStore _navigationStore;
        private readonly IViewModelLocator _viewModelLocator;
        private readonly ViewModelState _viewModelState;

        public NavigationService(NavigationStore navigationStore, IViewModelLocator viewModelLocator, ViewModelState viewModelState)
        {
            _navigationStore = navigationStore;
            _viewModelLocator = viewModelLocator;
            _viewModelState = viewModelState;
        }



        public NavigationStore GetNavigationStore()
        {
            return _navigationStore;
        }


        public async Task<ViewModelBase> NavigateAsync<T>(params Parameter[] parameters) where T : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelLocator.Create<T>(parameters);
            await NavigateAsync(viewModel);
            return viewModel;
        }

        public ViewModelBase Navigate<T>(params Parameter[] parameters) where T : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelLocator.Create<T>(parameters);
            Navigate(viewModel);
            return viewModel;
        }

        public void Navigate(ViewModelBase viewModel)
        {
            SetNavigationState(viewModel, true);
        }

        public async Task NavigateAsync(ViewModelBase viewModel)
        {

            _viewModelState.IsLoading = true;
            bool _canDisplay = await Task.Run(() => viewModel.ReadyToDisplay());
            SetNavigationState(viewModel, _canDisplay);
            _viewModelState.IsLoading = false;

        }

        public void SetNavigationState(ViewModelBase viewModel, bool canDisplay)
        {
            if (canDisplay) _navigationStore.CurrentViewModel = viewModel;
            else
            {
                viewModel.Dispose();
                System.Windows.MessageBox.Show("Cannot display the viewmodel");
            }
        }
    }
}
