using Autofac.Core;

namespace X_Guide.MVVM.ViewModel
{
    public interface IViewModelLocator
    {
        ViewModelBase Create<T>(string scopeName = null, params Parameter[] parameters) where T : ViewModelBase;
    }
}