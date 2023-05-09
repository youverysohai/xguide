using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step6.xaml
    /// </summary>
    public partial class Step6 : UserControl
    {
        private readonly bool isLive = false;

        public Step6()
        {
            InitializeComponent();
            Loaded += Step6_Loaded;
        }

        private void Step6_Loaded(object sender, RoutedEventArgs e)
        {
            ((Step6ViewModel)(DataContext)).OutputHandle = OutputWindow.HalconWindow;
            
        }

        private void Snap_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}