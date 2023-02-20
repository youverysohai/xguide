using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class TestingView : UserControl
    {
   /*     VmProcedure p;*/
        public TestingView()
        {
            InitializeComponent();
/*            VmSolution.Import(@"C:\Users\Xlent_XIR02\Desktop\test.sol", "");
            p = (VmProcedure)VmSolution.Instance["Flow1"];*/


        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
          /*  p_box.ModuleSource = p;
            p.Run();*/
        }
    }
}


