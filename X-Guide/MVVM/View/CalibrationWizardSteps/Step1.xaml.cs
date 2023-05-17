using System.Windows;
using System.Windows.Controls;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step1.xaml
    /// </summary>
    public partial class Step1 : UserControl
    {
        public Step1()
        {
            InitializeComponent();
        }

        private void ManipulatorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box)
            {
                box.SelectedItem = null;
                if (MessageBox.Show("WANT TO CHANGE?", null, MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                };
            }
        }
    }
}