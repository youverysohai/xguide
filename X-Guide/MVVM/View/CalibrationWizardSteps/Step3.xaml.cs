using System.Windows;
using System.Windows.Forms;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step3.xaml
    /// </summary>
    public partial class Step3 : System.Windows.Controls.UserControl
    {
        public Step3()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Vision solution files (*.sol)|*.sol|All files (*.*)|*.*";
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select a solution file";


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((Step3ViewModel)DataContext).FilePath = dialog.FileName;
            }
        }
    }
}
