using MaterialDesignThemes.Wpf;
using ModernWpf;
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

namespace X_Vision.Views
{
    /// <summary>
    /// Interaction logic for SkinView.xaml
    /// </summary>
    public partial class SkinView : UserControl
    {
        public SkinView()
        {
            InitializeComponent();
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

                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    //{
                    //    Source = new Uri("/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.Relative)
                    //}); 
                }
                else
                {
                    tm.ApplicationTheme = ApplicationTheme.Dark;
                    handyTM.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Dark;

                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    //{
                    //    Source = new Uri("/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Relative)
                    //}); 
                }
            });
        }
    }
}
