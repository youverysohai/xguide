using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MappingConfiguration;
using X_Guide.MVVM;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using Xlent_Vision_Guided;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        private readonly NavigationStore _navigationStore;
        private readonly NavigationStore _wizardNavigationStore;
        private Dictionary<PageName, Func<ViewModelBase>> _navDictionary;
        private DbContextFactory _dbContextFactory;
        private IUserService _userProvider;
        private IServerService _serverService;
        private ResourceDictionary _resourceDictionary;
        private IMachineService _machineDb;
        private MapperConfiguration _mapperConfig;
        private IClientService _clientService;
        private IContainer _diContainer;



        public App()
        {



            _mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MachineProfile>();
            }
            );



            //App specific settings
            InitializeAppConfiguration();

            _navigationStore = new NavigationStore();
            _wizardNavigationStore = new NavigationStore();


            //Navigation setting      
            _diContainer = BuildDIContainer();

        }

        IContainer BuildDIContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

          
            builder.Register(c => new ViewModelLocator(_diContainer)).As<IViewModelLocator>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>();

            builder.Register(c => new MainWindow()
            {
                DataContext = c.Resolve<MainViewModel>()
            });

            builder.RegisterType<NavigationStore>().SingleInstance();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<CalibrationWizardStartViewModel>();
            builder.RegisterType<Step1ViewModel>();
            builder.RegisterType<Step2ViewModel>();
            builder.RegisterType<Step3ViewModel>();
            builder.RegisterType<Step4ViewModel>();
            builder.RegisterType<Step5ViewModel>();
            builder.RegisterType<Step6ViewModel>();
            builder.RegisterType<SettingViewModel>();
/*
            builder.RegisterType<NavigationService>().As<INavigationService>().WithParameter(new TypedParameter(typeof(IContainer), _diContainer));*/
            builder.RegisterType<CalibrationMainViewModel>();
  
            builder.RegisterInstance(_mapperConfig.CreateMapper());
      
            builder.RegisterType<DbContextFactory>();
            builder.RegisterType<MachineService>().As<IMachineService>();
            builder.RegisterType<UserService>().As<IUserService>();
           
            builder.Register(c => new ServerService(IPAddress.Parse("192.168.10.92"), 8000, "\r\n")).As<IServerService>().SingleInstance();
            builder.Register(c => new ClientService(IPAddress.Parse("192.168.10.90"), 8000)).As<IClientService>().SingleInstance();

            return builder.Build();
        }
      
        private void InitializeAppConfiguration()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingPath = Path.Combine(appDataPath, "X-Guide", "Settings.xml");
            ConfigurationManager.AppSettings["SettingPath"] = settingPath;
        }
        //        Startup Page



        protected override void OnStartup(StartupEventArgs e)
        {




            _diContainer = BuildDIContainer();
            MainWindow = _diContainer.Resolve<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
     

    


    }

}
