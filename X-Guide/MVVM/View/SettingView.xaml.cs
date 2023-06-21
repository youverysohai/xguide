using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private async void ShowManipulatorDialogButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingViewModel).Manipulator = new ManipulatorViewModel();
            await ManipulatorDialog.ShowAsync();
        }

        private void ManipulatorDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }
    }
}