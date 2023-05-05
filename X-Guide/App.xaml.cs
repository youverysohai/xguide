using Autofac;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using X_Guide.Communication.Service;
using X_Guide.MappingConfiguration;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service;
using X_Guide.Service.Communation;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IContainer _diContainer = BuildDIContainer();
        //TODO: Add logger

        private static IContainer BuildDIContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(c => new ViewModelLocator(_diContainer)).As<IViewModelLocator>().SingleInstance();
            builder.Register(c => new MainWindow()
            {
                DataContext = c.Resolve<MainViewModel>()
            });

            builder.Register(c => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)).As<Configuration>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ManipulatorProfile>();
                cfg.AddProfile<CalibrationProfile>();
                cfg.AddProfile<VisionProfile>();
                cfg.AddProfile<GeneralProfile>();
            })).SingleInstance();
            builder.Register(c => new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));
                cfg.Dispatcher = Application.Current.Dispatcher;
            })).SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).SingleInstance();

            builder.Register(c => LoggerFactory.Create(b =>
            {
                b.AddSerilog(new LoggerConfiguration().WriteTo.Console().CreateLogger());
            })).As<ILoggerFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger<App>()).As<Microsoft.Extensions.Logging.ILogger>().SingleInstance();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<CalibrationWizardStartViewModel>();
            builder.RegisterType<Step1ViewModel>();
            builder.RegisterType<Step2ViewModel>();
            builder.RegisterType<Step3ViewModel>();
            builder.RegisterType<Step4ViewModel>();
            builder.RegisterType<Step5ViewModel>();
            builder.RegisterType<Step6ViewModel>();
            builder.RegisterType<SettingViewModel>();
            builder.RegisterType<CalibrationMainViewModel>();
            builder.RegisterType<HalconLive>();
            builder.RegisterType<HalconStep6>();
            builder.RegisterType<ViewModelState>().SingleInstance();
            builder.RegisterType<DbContextFactory>().SingleInstance();
            builder.RegisterType<ManipulatorDb>().As<IManipulatorDb>();
            builder.RegisterType<UserDb>().As<IUserDb>();
            builder.RegisterType<HIKVisionService>().As<IVisionService>();
            //builder.RegisterType<HalcomVisionService>().Named<IVisionService>("halcom");
            builder.RegisterType<JogService>().As<IJogService>();
            builder.RegisterType<ServerCommand>().SingleInstance();
            builder.RegisterType<NavigationStore>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<CalibrationService>().As<ICalibrationService>();
            builder.RegisterType<VisionDb>().As<IVisionDb>();
            builder.RegisterType<CalibrationDb>().As<ICalibrationDb>();
            builder.RegisterType<GeneralDb>().As<IGeneralDb>();

            builder.RegisterType<ServerService>().As<IServerService>().WithParameter(new TypedParameter(typeof(IPAddress), IPAddress.Parse("192.168.10.90"))).WithParameter(new TypedParameter(typeof(int), 8000)).WithParameter(new TypedParameter(typeof(string), "\r\n")).SingleInstance();
            builder.Register(c => new ClientService(IPAddress.Parse("192.168.10.90"), 7900, "")).As<IClientService>().SingleInstance();
            return builder.Build();
        }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //App specific settings
            InitializeAppConfiguration();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is CriticalErrorException ex)
            {
                HandyControl.Controls.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
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
            var visionService = _diContainer.Resolve<IVisionService>();
            MainWindow = _diContainer.Resolve<MainWindow>();
            _ = _diContainer.Resolve<ServerCommand>();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}