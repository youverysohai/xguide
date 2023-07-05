using HandyControl.Tools.Command;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Vision.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand<string> BackCommand { get; set; }


        public MainWindowViewModel(IRegionManager regionManager)
        {
            NavigateCommand = new DelegateCommand<string>(Navigate);
            BackCommand = new DelegateCommand<string>(Back);
            this.regionManager = regionManager;
        }

        private void Back(string obj)
        {
            if (journal.CanGoBack)
            {
                journal.GoBack();

            }
        }

        private void Navigate(string obj)
        {
            NavigationParameters keys = new NavigationParameters();
            keys.Add("Title", "Hello");
            regionManager.Regions["ContentRegion"].RequestNavigate(obj, callBack =>
            {
                if ((bool)callBack.Result)
                {
                    journal = callBack.Context.NavigationService.Journal;
                }
            }, keys);
        }
    }
}
