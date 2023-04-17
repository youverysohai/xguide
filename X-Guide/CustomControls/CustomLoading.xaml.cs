using MaterialDesignThemes.Wpf;
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
using ModernWpf.Controls;
using HandyControl.Tools.Extension;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomLoading.xaml
    /// </summary>
    public partial class CustomLoading : UserControl
    {




        public CustomLoading()
        {

            InitializeComponent();
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { OpenDialogBtn.ShowDialog(((DataTemplate)FindResource("template1")).LoadContent()); MessageBox.Show("Press ok to close the dialog"); }));
            Application.Current.Dispatcher.BeginInvoke(new Action(() => CloseDialogBtn.Command.Execute(null)));

        }
    }
}
