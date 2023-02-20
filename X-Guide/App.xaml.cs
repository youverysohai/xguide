using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        


        public App()
        {

            InitializeAppConfiguration();
            _setting = Setting.ReadFromXML(ConfigurationManager.AppSettings["SettingPath"]);
            _navigationStore = new NavigationStore();
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
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private SettingViewModel CreateSettingViewModel()
        {
            return new SettingViewModel(_setting, new NavigationService(_navigationStore, CreateTestingViewModel));
        }

        private TestingViewModel CreateTestingViewModel()
        {
            return new TestingViewModel(new NavigationService(_navigationStore, CreateSettingViewModel));
        }
    }

}
