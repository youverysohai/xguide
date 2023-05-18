using Autofac;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Configuration;
using System.Net;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using X_Guide.Communication.Service;
using X_Guide.MappingConfiguration;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.Model;
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
        private static IContainer _diContainer;
        public static Notifier Notifier;
        private static GeneralModel generalSetting;
        private static HikVisionModel hikSetting;

        //TODO: Add logger
        public static int VisionSoftware = 1;

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
                    parentWindow: Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));
                cfg.Dispatcher = Current.Dispatcher;
            })).SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).SingleInstance();

            builder.Register(c => LoggerFactory.Create(b =>
            {
                b.AddSerilog(new LoggerConfiguration().WriteTo.Console().CreateLogger());
            })).As<ILoggerFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger<App>()).As<Microsoft.Extensions.Logging.ILogger>().SingleInstance();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<CalibrationWizardStartViewModel>();
            builder.RegisterType<OperationViewModel>();
            builder.RegisterType<Step1ViewModel>();
            builder.RegisterType<Step2ViewModel>();
            builder.RegisterType<Step3HikViewModel>();
            builder.RegisterType<Step4ViewModel>();
            builder.RegisterType<Step5ViewModel>();
            builder.RegisterType<Step6ViewModel>();
            builder.RegisterType<SettingViewModel>();
            builder.RegisterType<CalibrationMainViewModel>();
            builder.RegisterType<ViewModelState>().SingleInstance();
            builder.RegisterType<DbContextFactory>().SingleInstance();
            builder.RegisterType<ManipulatorDb>().As<IManipulatorDb>();
            builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
            builder.RegisterType<UserDb>().As<IUserDb>();
            VisionSoftware = 3;
            switch (VisionSoftware)
            {
                case 1:
                    builder.RegisterType<HikVisionService>().As<IVisionService>().WithParameter(new TypedParameter(typeof(string), hikSetting.Filepath)).SingleInstance();
                    builder.RegisterType<HikViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikVisionDb>().As<IVisionDb>();

                    break;

                case 2:
                    builder.RegisterType<HalconVisionService>().As<IVisionService>().SingleInstance();
                    builder.RegisterType<HalconViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<LegacyVisionDb>().As<IVisionDb>();
                    break;

                case 3:
                    builder.RegisterType<SmartCamVisionService>().As<IVisionService>().SingleInstance();
                    builder.RegisterType<SmartCamViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikVisionDb>().As<IVisionDb>();
                    break;

                default: break;
            }

            builder.RegisterType<JogService>().As<IJogService>();
            builder.RegisterType<ServerCommand>().SingleInstance();
            builder.RegisterType<NavigationStore>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<CalibrationService>().As<ICalibrationService>();

            builder.RegisterType<CalibrationDb>().As<ICalibrationDb>();
            builder.RegisterType<GeneralDb>().As<IGeneralDb>();

            builder.RegisterType<ServerService>().As<IServerService>().WithParameter(new TypedParameter(typeof(IPAddress), IPAddress.Parse(generalSetting.Ip))).WithParameter(new TypedParameter(typeof(int), generalSetting.Port)).WithParameter(new TypedParameter(typeof(string), generalSetting.Terminator)).SingleInstance();

            builder.Register(c => new ClientService(IPAddress.Parse(hikSetting.Ip), hikSetting.Port, hikSetting.Terminator)).As<IClientService>().SingleInstance();

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

        private static void InitializeAppConfiguration()
        {
            //string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string settingPath = Path.Combine(appDataPath, "X-Guide", "Settings.xml");
            //ConfigurationManager.AppSettings["SettingPath"] = settingPath;
            var mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<GeneralProfile>();
                c.AddProfile<VisionProfile>();
            }).CreateMapper();
            var _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            generalSetting = mapper.Map<GeneralModel>((GeneralConfiguration)_configuration.GetSection("GeneralSetting"));
            hikSetting = mapper.Map<HikVisionModel>((HikVisionConfiguration)_configuration.GetSection("HikVisionSetting"));

            VisionSoftware = generalSetting.VisionSoftware;

            _diContainer = BuildDIContainer();
            Notifier = _diContainer.Resolve<Notifier>();
        }

        //Startup Page

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _diContainer.Resolve<MainWindow>();

            base.OnStartup(e);
            ViewModelState viewModelState = _diContainer.Resolve<ViewModelState>();
            try
            {
                _ = _diContainer.Resolve<IVisionService>();
                _ = _diContainer.Resolve<ServerCommand>();
            }
            catch
            {
                viewModelState.IsCalibValid = false;
            }
            finally
            {
                MainWindow.Show();
                if (!viewModelState.IsCalibValid)
                    Notifier.ShowError(StrRetriver.Get("VI003"));
            }
        }
    }
}