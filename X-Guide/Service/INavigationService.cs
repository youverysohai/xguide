using Autofac;
using Autofac.Core;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service
{
    public interface INavigationService
    {
        Task NavigateAsync(ViewModelBase viewModelBase);

        void Navigate(ViewModelBase viewModelBase);

        Task<ViewModelBase> NavigateAsync<T>(params Parameter[] parameters) where T : ViewModelBase;

        ViewModelBase Navigate<T>(params Parameter[] parameters) where T : ViewModelBase;

        void SetScope(ILifetimeScope lifetimeScope);

        NavigationStore GetNavigationStore();
    }
}