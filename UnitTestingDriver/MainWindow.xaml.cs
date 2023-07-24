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
     
        public MainWindow()
        {
           
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private Task BlockingCall(int i)
        {
            MessageBox.Show($"Current index = {i}");
            return Task.CompletedTask;
        }
    }
}