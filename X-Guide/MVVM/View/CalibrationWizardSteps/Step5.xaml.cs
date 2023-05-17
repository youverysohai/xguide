using System.Windows.Controls;
using Windows.Globalization.NumberFormatting;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5.xaml
    /// </summary>
    ///

    public partial class Step5 : UserControl
    {
        public Step5()
        {
            InitializeComponent();

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 2;

            //FormattedNumberBox.NumberFormatter = (ModernWpf.)formatter;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void RadialMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}