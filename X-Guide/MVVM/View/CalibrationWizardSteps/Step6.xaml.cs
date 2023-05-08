using HalconDotNet;
using IMVSCircleFindModuCs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using VM.Core;
using VMControls.WPF.Release;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step6.xaml
    /// </summary>
    public partial class Step6 : UserControl
    {


        bool isLive = false;

        public Step6()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                ((Step6ViewModel)(DataContext)).WindowHandle = HalconWindow.HalconWindow;
                ((Step6ViewModel)(DataContext)).OutputHandle = OutputWindow.HalconWindow;
            };

        }


        private void Snap_Click(object sender, RoutedEventArgs e)
        {
          
        }



    }
}
