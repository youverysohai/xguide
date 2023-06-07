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
using X_Guide.MVVM.ViewModel;

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
        private async void DisplayRemoveUserFormDialog()
        {
            await RemoveUserDialog.ShowAsync();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayEditUserFormDialog();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayRemoveUserFormDialog();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ((UserManagementViewModel)DataContext).InputPassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
