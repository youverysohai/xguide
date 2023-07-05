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
using X_Vision.Profiles;
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
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProcessModuleView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CameraModuleProfile>();
            moduleCatalog.AddModule<ProcessModuleProfile>();
            moduleCatalog.AddModule<CommunicationModuleProfile>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }

    }
}
