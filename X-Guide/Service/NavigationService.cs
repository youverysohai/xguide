using Autofac;
using Autofac.Core;
using CommunityToolkit.Mvvm.Messaging;
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
        private readonly StateViewModel _viewModelState;
        private readonly IMessenger _messenger;
        private ILifetimeScope _lifeTimeScope;

        public NavigationService(NavigationStore navigationStore, ILifetimeScope lifeTimeScope, StateViewModel viewModelState, IMessenger messenger)
        {
            _navigationStore = navigationStore;
            _lifeTimeScope = lifeTimeScope;
            _viewModelState = viewModelState;
            _messenger = messenger;
        }

        public void SetScope(ILifetimeScope lifetimeScope)
        {
            _lifeTimeScope = lifetimeScope;
        }

        public NavigationStore GetNavigationStore()
        {
            return _navigationStore;
        }

        public async Task<ViewModelBase> NavigateAsync<T>(params Parameter[] parameters) where T : ViewModelBase
        {
            _viewModelState.IsLoading = true;

            ViewModelBase viewModel = _lifeTimeScope.Resolve<T>(parameters);
            await Task.Run(_messenger.Send<ReadyRequest>);

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
            ViewModelBase viewModel = _lifeTimeScope.Resolve<T>(parameters);
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