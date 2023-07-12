using HandyControl.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for EngineeringView.xaml
    /// </summary>
    public partial class CalibrationMainView : UserControl
    {
        public CalibrationMainView()
        {
            InitializeComponent();
        }

        //private void Button_Prev(object sender, RoutedEventArgs e)
        //{
        //    step.Prev();
        //}

        //private void Button_Next(object sender, RoutedEventArgs e)
        //{
        //    step.Next();
        //}

        private async void step_StepChanged(object sender, FunctionEventArgs<int> e)
        {
            CalibrationMainViewModel viewModel = DataContext as CalibrationMainViewModel;
            if (viewModel != null)
            {
                viewModel.OnIndexChanged(e);
            }

        }

        private async Task RefreshScreen()
        {
            await Task.Delay(5000);
            InvalidateVisual();
        }

        //private void BtnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    ContentControl.Visibility = Visibility.Visible;
        //    StartStepPage.Visibility = Visibility.Collapsed;
        //}
    }
}