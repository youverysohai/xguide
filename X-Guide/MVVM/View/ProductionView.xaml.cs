using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.WPF.Release;
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
            //VmSolution.Import(@"Cest.sol", "");
            //p = (VmProcedure)VmSolution.Instance["Flow1"];


        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                VmSolution.Import(@"C:\Users\Xlent-Tung\Desktop\livecam.sol", "");
                p = (VmProcedure)VmSolution.Instance["Flow1"];
                p.Run();
                p_box.LoadFrontendSource();
                //p_box.BindSingleProcedure(p.ToString());
                p_box.AutoChangeSize();
            }
            catch (VmException ex)
            {

            }



        }


    }
}


