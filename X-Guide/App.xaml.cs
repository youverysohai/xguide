using Autofac;
using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
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
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;
using X_Guide.VisionMaster;
using ILogger = Serilog.ILogger;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _diContainer;
        public static Notifier Notifier;
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.File($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/X-Guide/Error_Log.txt").CreateLogger();

        //TODO: Add logger
        public static int VisionSoftware = 1;

        private static IContainer BuildDIContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<WeakReferenceMessenger>().As<IMessenger>().SingleInstance();
            builder.RegisterType<DisposeService>().As<IDisposeService>().SingleInstance();
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
                cfg.AddProfile<UserProfile>();
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
            builder.RegisterType<UserManagementViewModel>();
            builder.RegisterType<CalibrationMainViewModel>();
            builder.RegisterType<JogRobotViewModel>();
            builder.RegisterType<LiveCameraViewModel>();
            builder.RegisterType<StateViewModel>().SingleInstance();
            builder.RegisterType<DbContextFactory>().SingleInstance();
            builder.RegisterType<ManipulatorDb>().As<IManipulatorDb>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();

            builder.RegisterType<UserDb>().As<IUserDb>();
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterType<JsonDb>().As<IJsonDb>();
            builder.Register(c =>
            {
                var db = c.Resolve<IJsonDb>().Get<HikVisionModel>();
                return new ClientService(IPAddress.Parse(db.Ip), db.Port, db.Terminator);
            }).As<IClientService>().SingleInstance();
            switch (VisionSoftware)
            {
                case 1:
                    builder.Register(c => new HikVisionService(c.Resolve<IClientService>(), c.Resolve<IJsonDb>().Get<HikVisionModel>().Filepath)).As<IVisionService>();
                    builder.RegisterType<HikViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikOperationService>().As<IOperationService>();

                    break;

                case 2:
                    builder.RegisterType<HalconVisionService>().As<IVisionService>().SingleInstance();
                    builder.RegisterType<HalconViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HalconOperationService>().As<IOperationService>();
                    break;

                case 3:
                    builder.RegisterType<SmartCamVisionService>().As<IVisionService>().SingleInstance();
                    builder.RegisterType<SmartCamViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<SmartCamOperationService>().As<IOperationService>();
                    break;

                default: break;
            }

            builder.RegisterType<JogService>().As<IJogService>();
            builder.RegisterType<ServerCommand>().As<IServerCommand>().SingleInstance();
            builder.RegisterType<NavigationStore>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<CalibrationService>().As<ICalibrationService>();

            builder.RegisterType<CalibrationDb>().As<ICalibrationDb>();
            builder.RegisterType<GeneralDb>().As<IGeneralDb>();

            builder.Register(c =>
            {
                var db = c.Resolve<IJsonDb>().Get<GeneralModel>();
                return new ServerService(IPAddress.Parse(db.Ip), db.Port, db.Terminator, c.Resolve<IMessenger>());
            }).As<IServerService>().SingleInstance();

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
            logger.Error(e.ExceptionObject.ToString());
            if (e.ExceptionObject is CriticalErrorException ex)
            {
                HandyControl.Controls.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        private static void InitializeAppConfiguration()
        {
            var mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<GeneralProfile>();
                c.AddProfile<VisionProfile>();
            }).CreateMapper();
            IJsonDb jsonDb = new JsonDb(mapper);
            VisionSoftware = jsonDb.Get<GeneralModel>().VisionSoftware;
            _diContainer = BuildDIContainer();
            _ = _diContainer.Resolve<IMessageBoxService>();
            Notifier = _diContainer.Resolve<Notifier>();
        }

        //Startup Page

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _diContainer.Resolve<MainWindow>();

            base.OnStartup(e);
            StateViewModel viewModelState = _diContainer.Resolve<StateViewModel>();

            try
            {
                _ = _diContainer.Resolve<IVisionService>();
                _ = _diContainer.Resolve<IServerCommand>();
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

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _diContainer.Resolve<IDisposeService>().Dispose();
        }
    }
}