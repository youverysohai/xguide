using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using X_Vision.ViewModels;
using X_Vision.Views;

namespace X_Vision
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<ProcessModuleView, ProcessModuleViewModel>();
            containerRegistry.RegisterForNavigation<CameraModuleView, CameraModuleViewModel>();
            containerRegistry.RegisterForNavigation<CommunicationModuleView, CommunicationModuleViewModel>();
            containerRegistry.RegisterForNavigation<SettingsModuleView, SettingsModuleViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

        }

    }
}
