using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using ManipulatorTcp;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using TcpConnectionHandler;
using TcpConnectionHandler.Client;
using TcpConnectionHandler.Server;
using TcpVisionProvider;
using VisionProvider.Interfaces;

namespace UnitTestingDriver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICalibrationService calibrationService;
        private readonly IVisionService visionService;
        private readonly IJogService jogService;
        private readonly ServerTcp serverTcp;
        private readonly IClientTcp clientTcp;
        private readonly WeakReferenceMessenger messenger;

        public MainWindow()
        {
            messenger = new WeakReferenceMessenger();
            TcpConfiguration clientTcpConfiguration = new TcpConfiguration
            {
                IPAddress = IPAddress.Parse("192.168.10.90"),
                Port = 8080,
                Terminator = "",
            };
            TcpConfiguration serverTcpConfiguration = new TcpConfiguration
            {
                IPAddress = IPAddress.Parse("192.168.10.90"),
                Port = 8001,
                Terminator = "",
            };
            serverTcp = new ServerTcp(serverTcpConfiguration);
            serverTcp.Start();
            clientTcp = new ClientTcp(clientTcpConfiguration);
            clientTcp.ConnectServer();
            jogService = new JogService(serverTcp);
            visionService = new TcpVisionService(clientTcp, messenger);
            calibrationService = new CalibrationService(visionService, jogService, messenger);
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            messenger.Register<ReadyProceed>(this, (r, m) =>
            {
                MessageBox.Show("Ready!");
                m.Reply(true);
                if (!m.Ready) messenger.Unregister<ReadyProceed>(this);
            });
            _ = Task.Run(calibrationService.TopConfig9Point);
        }
    }
}