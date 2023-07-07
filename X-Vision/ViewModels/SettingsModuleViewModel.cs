using HandyControl.Tools.Command;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Vision.Common.Models;
using X_Vision.Extensions;

namespace X_Vision.ViewModels
{
    public class SettingsModuleViewModel:BindableBase
    {
        private readonly IRegionManager regionManager;
        public SettingsModuleViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
                return;
            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(bar.NameSpace);
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    }
}
