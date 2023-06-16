using System;
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
            ((Step6ViewModel)DataContext).OnCalibrationChanged += ShowDialog;
        }

        private void ShowDialog(object sender, EventArgs e)
        {
            DisplayNewCalibrationDialog();
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
            if (!((Step6ViewModel)DataContext).IsFirstCalibResult)
            {
                DisplayNewCalibrationDialog();
            }
        }
    }
}