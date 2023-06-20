using Autofac.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.MessageToken;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.State;

namespace X_Guide.Service
{
    public class NavigationService : INavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly IViewModelLocator _viewModelLocator;
        private readonly StateViewModel _viewModelState;
        private readonly IMessenger _messenger;
        private readonly ManualResetEventSlim _manualResetEvent;

        public NavigationService(NavigationStore navigationStore, IViewModelLocator viewModelLocator, StateViewModel viewModelState, IMessenger messenger)
        {
            _navigationStore = navigationStore;
            _viewModelLocator = viewModelLocator;
            _viewModelState = viewModelState;
            _messenger = messenger;
        }

        public NavigationStore GetNavigationStore()
        {
            return _navigationStore;
        }

        public async Task<ViewModelBase> NavigateAsync<T>(params Parameter[] parameters) where T : ViewModelBase
        {
            _viewModelState.IsLoading = true;

            ViewModelBase viewModel = _viewModelLocator.Create<T>(parameters);
            await Task.Factory.StartNew(() => _messenger.Send<ReadyRequest>());

            _viewModelState.IsLoading = false;
            SetNavigationState(viewModel, true);

            return viewModel;
        }

        public Task NavigateAsync(ViewModelBase viewModelBase)
        {
            SetNavigationState(viewModelBase, true);
            return Task.CompletedTask;
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