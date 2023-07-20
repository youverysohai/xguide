using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for CalibrationWizardStart.xaml
    /// </summary>
    public partial class CalibrationWizardStart : UserControl
    {
        private readonly SnackbarMessageQueue messageQueue = new SnackbarMessageQueue();

        public CalibrationWizardStart()
        {
            InitializeComponent();

            SnackbarMessage.MessageQueue = messageQueue;
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            Chip chip = (Chip)sender;
            messageQueue.Enqueue(chip.Content + " is Deleted", null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(1.55));

            if (chip != null && chip.DataContext is CalibrationViewModel calibration)
            {
                var viewModel = DataContext as CalibrationWizardStartViewModel;
                if (viewModel != null)
                {
                    viewModel.DeleteCalibration(calibration);
                }
            }
        }
    }
}