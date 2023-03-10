using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;

namespace X_Guide.MVVM.Command
{
    internal class WizPrevCommand : CommandBase
    {
        public EngineeringViewModel _viewModel { get; }
        public NavigationStore _navigationStore { get; }
        public NavigationService _navigationService;
        public WizPrevCommand(EngineeringViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _navigationService = new NavigationService(_navigationStore, null);
        }
        public override void Execute(object parameter)
        {


            LinkedListNode<ViewModelBase> prevNode = _viewModel.CurrentNode.Previous;

            if (prevNode != null)
            {
                _navigationService.Navigate(prevNode.Value);
                _viewModel.CurrentNode = prevNode;
                _viewModel.CurrentStep -= 1;
            }
            else
            {

                MessageBox.Show("This is the first page!");
            }

        }
    }
}
