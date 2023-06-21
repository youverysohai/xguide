using ModernWpf;
using System;
using System.Windows;
using System.Windows.Controls;
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
            //DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            //{
            //    this.DateTimeTextBlock.Text = DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss");
            //}, this.Dispatcher);
        }

        private async void DisplaySignUpDialog()
        {
            await RegisterDialog.ShowAsync();
        }

        private async void DisplayLoginDialog()
        {
            await LoginDialog.ShowAsync();
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
                    BrightIcon.Visibility = Visibility.Visible;
                    DarkIcon.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tm.ApplicationTheme = ApplicationTheme.Dark;
                    handyTM.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Dark;

                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Relative)
                    });
                    BrightIcon.Visibility = Visibility.Collapsed;
                    DarkIcon.Visibility = Visibility.Visible;
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

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            DisplaySignUpDialog();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ((MainViewModel)DataContext).InputPassword = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayLoginDialog();
        }
    }
}