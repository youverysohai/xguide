using System.Windows;
using System.Windows.Controls;

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
        }

        private void Snap_Click(object sender, RoutedEventArgs e)
        {
        }
        private async void DisplayNewCalibrationDialog()
        {
            await NewCalibrationDialog.ShowAsync();
        }

        private void StartCalibrationBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayNewCalibrationDialog();
        }
    }
}