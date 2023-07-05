using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Vision.ViewModels;
using X_Vision.Views;

namespace X_Vision.Profiles
{
    public class CameraModuleProfile : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CameraModuleView,CameraModuleViewModel>();
        }
    }
}
