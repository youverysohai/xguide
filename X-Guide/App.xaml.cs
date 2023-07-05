
//using ToastNotifications.Messages;
using ILogger = Serilog.ILogger;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///
    [SupportedOSPlatform("windows")]
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
            builder.Register(c => new AuthenticationService(c.Resolve<IRepository>(), c.Resolve<IMessenger>())).SingleInstance();
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
            builder.RegisterType<Step3ViewModel>();
            builder.RegisterType<Step4ViewModel>();
            builder.RegisterType<Step5ViewModel>();
            builder.RegisterType<Step6ViewModel>();

            builder.RegisterType<SettingViewModel>();
            builder.RegisterType<UserManagementViewModel>();
            builder.RegisterType<CalibrationMainViewModel>();
            builder.RegisterType<JogRobotViewModel>();
            builder.RegisterType<LiveCameraViewModel>();
            builder.RegisterType<StateViewModel>().SingleInstance();
            builder.RegisterType<Repository>().As<IRepository>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterType<JsonDb>().As<IJsonDb>();
            builder.Register(c =>
            {
                var db = c.Resolve<IJsonDb>().Get<HikVisionModel>();
                var config = new TcpConfiguration
                {
                    IPAddress = IPAddress.Parse(db.Ip),
                    Port = db.Port,
                    Terminator = db.Terminator
                };
                return new ClientTcp(config);
            }).As<IClientTcp>().SingleInstance();

            switch (VisionSoftware)
            {
                case 1:
                    builder.Register(c =>
                    {
                        string filepath = c.Resolve<IJsonDb>().Get<HikVisionModel>().Filepath;
                        IClientTcp client = c.Resolve<IClientTcp>();
                        return new HikVisionService(filepath, client);
                    }
                    ).As<IVisionService>().SingleInstance();
                    builder.RegisterType<HikViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikVisionCalibrationStep>().As<IVisionCalibrationStep>();
                    builder.RegisterType<HikOperationService>().As<IOperationService>();
                    builder.RegisterType<HikVisionCalibrationStep>().As<IVisionCalibrationStep>();

                    break;

                case 2:

                    builder.RegisterType<HalconViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikVisionCalibrationStep>().As<IVisionCalibrationStep>();
                    builder.RegisterType<HalconOperationService>().As<IOperationService>();

                    break;

                case 3:

                    builder.RegisterType<SmartCamViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<HikVisionCalibrationStep>().As<IVisionCalibrationStep>();
                    builder.RegisterType<SmartCamOperationService>().As<IOperationService>();

                    break;

                case 4:

                    builder.RegisterType<TcpVisionService>().As<IVisionService>();
                    builder.RegisterType<SmartCamViewModel>().As<IVisionViewModel>();
                    builder.RegisterType<SmartCamOperationService>().As<IOperationService>();
                    builder.RegisterType<OthersVisionCalibrationStep>().As<IVisionCalibrationStep>();
                    break;

                default: break;
            }
            builder.RegisterType<OthersVisionCalibrationStep>();
            builder.RegisterType<JogService>().As<IJogService>();
            builder.RegisterType<ServerCommand>().As<IServerCommand>().SingleInstance();
            builder.RegisterType<NavigationStore>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<CalibrationService>().As<ICalibrationService>();
            builder.Register(c =>
            {
                var db = c.Resolve<IJsonDb>().Get<GeneralModel>();
                var config = new TcpConfiguration
                {
                    IPAddress = IPAddress.Parse(db.Ip),
                    Port = db.Port,
                    Terminator = db.Terminator,
                };
                return new ServerTcp(config);
            }).As<IServerTcp>().SingleInstance();

            return builder.Build();
        }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var i = Enum.GetValues(typeof(ManipulatorType));
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
            IJsonDb jsonDb = new JsonDb();
            VisionSoftware = jsonDb.Get<GeneralModel>().VisionSoftware;
            _diContainer = BuildDIContainer();
            _ = _diContainer.Resolve<IMessageBoxService>();
            _ = _diContainer.Resolve<AuthenticationService>();
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
                _ = _diContainer.Resolve<IServerCommand>();
            }
            catch (Exception ex)
            {
                viewModelState.IsCalibValid = false;
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                MainWindow.Show();
                //if (!viewModelState.IsCalibValid)
                //    Notifier.ShowError(StrRetriver.Get("VI003"));
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _diContainer.Resolve<IDisposeService>().Dispose();
        }
    }
}