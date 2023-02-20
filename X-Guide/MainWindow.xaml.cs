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
                this.DateTimeTextBlock.Text = DateTime.Now.ToString("dddd , MMM dd yyyy,hh:mm:ss");
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

    }
}
