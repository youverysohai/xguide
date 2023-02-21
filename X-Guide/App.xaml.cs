using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Setting _setting;
        private readonly NavigationStore _navigationStore;
        private  Dictionary<PageTitle, NavigationService> _viewModels;
        


        public App()
        {
            _navigationStore = new NavigationStore();
            _setting = Setting.ReadFromXML(ConfigurationManager.AppSettings["SettingPath"]);

            //App specific settings
            InitializeAppConfiguration();
     
            //Navigation setting      
            InitializeAppNavigation();


        }

     
        private void InitializeAppNavigation()
        {
            _viewModels = new Dictionary<PageTitle, NavigationService>
            {
                {PageTitle.Setting, new NavigationService (_navigationStore, CreateSettingViewModel) },
                {PageTitle.Production, new NavigationService (_navigationStore, CreateProductionViewModel) },
                {PageTitle.Engineering, new NavigationService (_navigationStore, CreateEngineeringViewModel) },
                {PageTitle.Security, new NavigationService (_navigationStore, CreateSecurityViewModel) },
                {PageTitle.Undefined, new NavigationService(_navigationStore, CreateUndefinedViewModel) }
            };
        }
        
        private void InitializeAppConfiguration()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingPath = Path.Combine(appDataPath, "X-Guide", "Settings.xml");
            ConfigurationManager.AppSettings["SettingPath"] = settingPath;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateSettingViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _viewModels)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }


        private ViewModelBase CreateSecurityViewModel()
        {
            return new SecurityViewModel();
        }
        private ViewModelBase CreateEngineeringViewModel()
        {
            return new EngineeringViewModel();
        }

        private ViewModelBase CreateUndefinedViewModel()
        {
            return new UndefinedViewModel();
        }

        private ViewModelBase CreateSettingViewModel()
        {
            return new SettingViewModel(_setting);
        }

        private ViewModelBase CreateProductionViewModel()
        {
            return new ProductionViewModel();
        }
    }

}
