using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

        public HikVisionViewModel Vision
        {
            get { return (HikVisionViewModel)GetValue(VisionProperty); }
            set { SetValue(VisionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vision.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisionProperty =
            DependencyProperty.Register("Vision", typeof(HikVisionViewModel), typeof(CustomVisionForm), new PropertyMetadata(null, VisionChanged));

        private static void VisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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