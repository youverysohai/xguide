using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Vision.Common.Models;
using X_Vision.Extensions;

namespace X_Vision.ViewModels
{
    public class MainViewModel : BindableBase
    {
 

        public MainViewModel(IRegionManager regionManager)
        {

            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            GoForwardCommand = new DelegateCommand(GoForward);
            this.regionManager = regionManager;
        }

        private void GoForward()
        {
            if (journal != null && journal.CanGoForward)
                journal.GoForward();
        }

        private void GoBack()
        {
            if (journal != null && journal.CanGoBack)
                journal.GoBack();
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
            {
                return;
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });


        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
         
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

    }
}
