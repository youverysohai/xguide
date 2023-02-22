using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;
using X_Guide.Service.UserProviders;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Setting _setting;
        private readonly NavigationStore _navigationStore;
        private  Dictionary<PageName, NavigationService> _viewModels;
        private DbContextFactory _dbContextFactory;
        private IUserService _userProvider;


        public App()
        {
            _dbContextFactory = new DbContextFactory();
             _userProvider = new DatabaseUserService(_dbContextFactory);

            //App specific settings
            InitializeAppConfiguration();

            _navigationStore = new NavigationStore();

            //Navigation setting      
            InitializeAppNavigation();


        }

     
        private void InitializeAppNavigation()
        {
            _viewModels = new Dictionary<PageName, NavigationService>
            {
                {PageName.Setting, new NavigationService (_navigationStore, CreateSettingViewModel) },
                {PageName.Production, new NavigationService (_navigationStore, CreateProductionViewModel) },
                {PageName.Engineering, new NavigationService (_navigationStore, CreateEngineeringViewModel) },
                {PageName.Security, new NavigationService (_navigationStore, CreateSecurityViewModel) },
                {PageName.Undefined, new NavigationService(_navigationStore, CreateUndefinedViewModel) }
            };

            _setting = Setting.ReadFromXML(ConfigurationManager.AppSettings["SettingPath"]);
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
            return new SettingViewModel(_setting, _userProvider);
        }

        private ViewModelBase CreateProductionViewModel()
        {
            return new ProductionViewModel();
        }
    }

}
