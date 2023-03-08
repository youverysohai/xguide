
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
                this.DateTimeTextBlock.Text = DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss");
            }, this.Dispatcher);
        }


        //Purpose: Double click Application for maximize window/Normal size 
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)      //Double Click 
                //Compare current window state, if maximize , then the window state is changed to normal or vice versa 
                WindowState = (WindowState == WindowState.Maximized) ?
                    WindowState.Normal : WindowState.Maximized;
            else
                DragMove(); //Drag the window 
        }

        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MaximizedButton_Clicked
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //MaxButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                //MaxButton.Content = "2";
            }

        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;

            foreach (Expander otherExpander in FindVisualChildren<Expander>(this))
            {
                if (otherExpander != expander)
                {
                    otherExpander.IsExpanded = false;
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a reference to the application resources
            ResourceDictionary resources = Application.Current.Resources;
            Color foregroundColor = (Color)resources["PrimaryTextColor"]; //white


            // Get the SolidColorBrush resource from the resource dictionary
            SolidColorBrush foregroundBrush = (SolidColorBrush)resources["PrimaryBlueColor"];
            SolidColorBrush foregroundWhiteBrush = new SolidColorBrush(foregroundColor);
            if (SideMenu.Width > 50)
            {
                TextElement.SetForeground(HambergerMenuBtn, foregroundWhiteBrush);
                SideMenu.Width = 50;
                TextTitle.Visibility = Visibility.Collapsed;
                Username.Visibility = Visibility.Collapsed;
                RoleIcon.Visibility = Visibility.Collapsed;
                UserControlContent.SetValue(Grid.ColumnProperty, 2);
                UserControlContent.SetValue(Grid.ColumnSpanProperty, 3);

            }
            else
            {
                TextElement.SetForeground(HambergerMenuBtn, foregroundBrush);
                SideMenu.Width = 180;
                TextTitle.Visibility = Visibility.Visible;
                Username.Visibility = Visibility.Visible;
                RoleIcon.Visibility = Visibility.Visible;
                UserControlContent.SetValue(Grid.ColumnProperty, 3);
                UserControlContent.SetValue(Grid.ColumnSpanProperty, 1);

            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Username.Text = "Hi, Admin";
            PackIconKind icon = (PackIconKind)System.Enum.Parse(typeof(PackIconKind), "AccountTieHatOutline");
          
            RoleIcon.Kind = icon;
            
            // Get a reference to the application resources
            ResourceDictionary resources = Application.Current.Resources;
            Color foregroundColor = (Color)resources["PrimaryTextColor"]; //white


            // Get the SolidColorBrush resource from the resource dictionary
            SolidColorBrush foregroundBrush = (SolidColorBrush)resources["PrimaryBlueColor"];
            SolidColorBrush foregroundWhiteBrush = new SolidColorBrush(foregroundColor);

            // Set the foreground color of the TextBlock using the brush
            TextElement.SetForeground(LogoutIcon, foregroundWhiteBrush);
            TextElement.SetForeground(Username, foregroundBrush);
            TextElement.SetForeground(TxtRoleIcon, foregroundBrush);
            LoginIcon.Visibility = Visibility.Collapsed;
            LogoutIcon.Visibility = Visibility.Visible;

        }


        private void LogoutIcon_Click(object sender, RoutedEventArgs e)
        {
            
            
            Username.Text = "Hi, Guest";
            PackIconKind icon = (PackIconKind)System.Enum.Parse(typeof(PackIconKind), "Redhat");

            RoleIcon.Kind = icon;

            // Get a reference to the application resources
            ResourceDictionary resources = Application.Current.Resources;

            // Get the SolidColorBrush resource from the resource dictionary
            Color foregroundColor = (Color)resources["PrimaryTextColor"];
            SolidColorBrush foregroundBrush = new SolidColorBrush(foregroundColor);

            // Set the foreground color of the TextBlock using the brush
            TextElement.SetForeground(Username, foregroundBrush);
            TextElement.SetForeground(TxtRoleIcon, foregroundBrush);
            LoginIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((MainViewModel)DataContext).InputPassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }

}

