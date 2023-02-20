using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VM.Core;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class TestingView : UserControl
    {
        VmProcedure p;
        public TestingView()
        {
               
            InitializeComponent();
           


        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
            VmSolution.Import(@"C:\Users\Xlent_XIR02\Desktop\test.sol", "");
            p = (VmProcedure)VmSolution.Instance["Flow1"];
            
            p_box.ModuleSource = p;
            p.Run();
        }

      
    }
}


