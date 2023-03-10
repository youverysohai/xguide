using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.View;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;

namespace X_Guide.MVVM.Command
{
    internal class WizNextCommand : CommandBase
    {


        public EngineeringViewModel _viewModel { get; }
        public NavigationStore _navigationStore { get; }
        public NavigationService _navigationService;

        public WizNextCommand(EngineeringViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _navigationService = new NavigationService(_navigationStore, null);
        }

        public override void Execute(object parameter)
        {
            LinkedListNode<ViewModelBase> nextNode = _viewModel.CurrentNode.Next;

            if (nextNode != null)
            {
                _navigationService.Navigate(nextNode.Value);
                _viewModel.CurrentNode = nextNode;
                _viewModel.CurrentStep += 1;
            }
            else
            {
                ViewModelBase viewModel = _viewModel.CurrentViewModel.GetNextViewModel();
                if (viewModel != null)
                {
                    _navigationService.Navigate(viewModel);
                    _viewModel.CurrentNode = _viewModel.NavigationHistory.AddLast(viewModel);
                    _viewModel.CurrentStep += 1;
                }
                else
                {
                    MessageBox.Show("This is the last page!");
                }
            }

        }
    }
}
