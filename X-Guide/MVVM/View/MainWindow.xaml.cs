
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using X_Guide.MVVM.ViewModel;

namespace X_Guide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.SideMenuNavigation.PaneTitle = DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss");
            }, this.Dispatcher);
        }




       


        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if(sender != null)
        //    {
        //        ((MainViewModel)DataContext).InputPassword = ((PasswordBox)sender).SecurePassword;
        //    }
        //}
    }

}

