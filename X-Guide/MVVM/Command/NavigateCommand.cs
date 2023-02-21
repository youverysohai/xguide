using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;

namespace X_Guide.MVVM.Command
{
    internal class NavigateCommand : CommandBase
    {

        private readonly Dictionary<PageTitle, NavigationService> _viewModels;

        public NavigateCommand(Dictionary<PageTitle, NavigationService> viewModels)
        {

            _viewModels = viewModels;
        }

        public override void Execute(object parameter)
        {
            if (parameter is PageTitle pageTitle && _viewModels.TryGetValue(pageTitle, out var page))
            {
                page.Navigate();
            }
                
        }
    }
}
