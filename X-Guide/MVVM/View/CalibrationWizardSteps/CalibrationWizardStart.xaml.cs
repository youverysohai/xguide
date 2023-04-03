using HandyControl.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for CalibrationWizardStart.xaml
    /// </summary>
    public partial class CalibrationWizardStart : UserControl
    {
        private SnackbarMessageQueue messageQueue = new SnackbarMessageQueue();



        public CalibrationWizardStart()
        {
            InitializeComponent();



            SnackbarMessage.MessageQueue = messageQueue;

        }


        private void Chip_Click(object sender, RoutedEventArgs e)
        {

            Chip chip = (Chip)sender;
            messageQueue.Enqueue(chip.Content + " is Selected", null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(1.55));
            CalibName.Text = (string)chip.Content;
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
