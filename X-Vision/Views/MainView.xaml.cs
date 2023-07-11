using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using X_Vision.Common.Models;

namespace X_Vision.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {

        private readonly static MenuBar[] menuBars = new MenuBar[]
{
            new MenuBar() { Icon = "HomeOutline", Title = "Process", NameSpace = "ProcessModuleView" },
            new MenuBar() { Icon = "CameraOutline", Title = "Camera", NameSpace = "CameraModuleView" }             ,
            new MenuBar() { Icon = "Connection", Title = "Communication", NameSpace = "CommunicationModuleView" },
            new MenuBar() { Icon = "WrenchCogOutline", Title = "Advanced Settings", NameSpace = "SettingsModuleView" },
            new MenuBar() { Icon = "FileCogOutline", Title = "Log File", NameSpace = "LogFileModuleView" }
};

        public MainView()
        {
            InitializeComponent();

            menuBar.ItemsSource = menuBars;
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnClose.Click += (s, e) => { this.Close(); };
            btnMax.Click += (s, e) => {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                    this.WindowState = WindowState.Maximized;

            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                    this.WindowState = WindowState.Normal;

            };
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

        }

        private void OpenFlyout_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void FolderContext_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {

        }
    }

}
