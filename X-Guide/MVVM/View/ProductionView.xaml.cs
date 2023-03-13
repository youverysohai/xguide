using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VM.Core;
//using VM.Core;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class ProductionView : UserControl
    {
        VmProcedure p;
        public ProductionView()
        {
               
            InitializeComponent();
           


        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
            VmSolution.Import(@"C:\Users\Xlent-Tung\Desktop\test.sol", "");
            p = (VmProcedure)VmSolution.Instance["Flow1"];

            p_box.ModuleSource = p;
            p.Run();
        }


    }
}


