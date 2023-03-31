
using MaterialDesignThemes.Wpf;
using ModernWpf;
using ModernWpf.Controls;
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
using X_Guide.MVVM.View;
using X_Guide.MVVM.View.CalibrationWizardSteps;
using X_Guide.MVVM.ViewModel;
using Microsoft.Xaml.Behaviors;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.MVVM;
using Windows.UI.WindowManagement;

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
                this.DateTimeTextBlock.Text = DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss");
            }, this.Dispatcher);

        }

        private void ToggleAppThemeHandler(object sender, RoutedEventArgs e)
        {
            ClearValue(ThemeManager.RequestedThemeProperty);

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var tm = ThemeManager.Current;
                var handyTM = HandyControl.Themes.ThemeManager.Current;
                
                if (tm.ActualApplicationTheme == ApplicationTheme.Dark)
                {
                    tm.ApplicationTheme = ApplicationTheme.Light;
                    handyTM.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Light;
                    // Set the Light theme ResourceDictionary
              
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.Relative)
                    });   
                }
                else
                {
                    tm.ApplicationTheme = ApplicationTheme.Dark;
                    handyTM.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Dark;
                     
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Relative)
                    });
                }
            });
        }

        private void ToggleWindowThemeHandler(object sender, RoutedEventArgs e)
        {
            if (ThemeManager.GetActualTheme(this) == ElementTheme.Light)
            {
                ThemeManager.SetRequestedTheme(this, ElementTheme.Dark);
            }
            else
            {
                ThemeManager.SetRequestedTheme(this, ElementTheme.Light);
            }
        }

        private void SideMenuNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // Handle the settings item click event here
                (DataContext as MainViewModel).NavigateCommand.Execute(PageName.Setting);
            }

            if (args.InvokedItemContainer.Tag != null)
            {
                string tag = args.InvokedItemContainer.Tag.ToString();

                switch (tag)
                {
                    case "ProductionView":
                        (DataContext as MainViewModel).NavigateCommand.Execute(PageName.Production);
                        break;
                    //case "CalibrationWizardStartView":
                    //    (DataContext as MainViewModel).NavigateCommand.Execute(PageName.CalibrationWizardStart);
                    //    break;
                    case "JogRobotView":
                        (DataContext as MainViewModel).NavigateCommand.Execute(PageName.JogRobot);
                        break;
                }
            }
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

