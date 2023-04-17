
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Serialization;
using Windows.UI.WindowManagement;
using X_Guide.CustomControls;
using X_Guide.MVVM.Model;
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
