using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using X_Guide.Enums;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomVisionForm.xaml
    /// </summary>
    public partial class CustomVisionForm : UserControl
    {
        public CustomVisionForm()
        {
            InitializeComponent();
        }

        public VisionViewModel Vision
        {
            get { return (VisionViewModel)GetValue(VisionProperty); }
            set { SetValue(VisionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vision.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisionProperty =
            DependencyProperty.Register("Vision", typeof(VisionViewModel), typeof(CustomVisionForm), new PropertyMetadata(null, VisionChanged));

        private static void VisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        
        }

        private void VisionSoftware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisionSoftware.SelectedIndex == 2)
            {
                // Hide the fields
                Fields.Visibility = Visibility.Collapsed;
                RestartIcon.Visibility = Visibility.Visible;
            }
            else
            {
                // Show the fields
                Fields.Visibility = Visibility.Visible;
                RestartIcon.Visibility = Visibility.Collapsed;

            }
        }

        private void RestartIcon_Click(object sender, RoutedEventArgs e)
        {
            // Start a new instance of the application
            Process.Start(Application.ResourceAssembly.Location);

            // Shutdown the current instance of the application
            Application.Current.Shutdown();
        }
    }
}
