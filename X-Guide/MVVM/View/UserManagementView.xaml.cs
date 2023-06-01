using MahApps.Metro.Controls.Dialogs;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
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

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserLoginView.xaml
    /// </summary>
    public partial class UserLoginView : UserControl
    {
        public UserLoginView()
        {
            InitializeComponent();
        }

        private async void DisplayEditUserFormDialog()
        {
            await EditUserDialog.ShowAsync();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayEditUserFormDialog();
        }
    }
}
