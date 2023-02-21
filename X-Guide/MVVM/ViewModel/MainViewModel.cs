using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    //displaying the current viewmodel of the application

    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;


        public ICommand NavigateCommand { get; set; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        public MainViewModel(NavigationStore navigationStore, Dictionary<PageTitle, NavigationService> viewModels)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateCommand = new NavigateCommand(viewModels);


        }

      

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }


    }
}
