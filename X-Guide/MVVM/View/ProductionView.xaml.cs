using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
//using VM.Core;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class ProductionView : UserControl
    {
        //VmProcedure p;
        public ProductionView()
        {
               
            InitializeComponent();
           


        }

        //private void p_box_Loaded(object sender, RoutedEventArgs e)
        //{
        //    VmSolution.Import(@"C:\Users\Xlent_XIR02\Desktop\test.sol", "");
        //    p = (VmProcedure)VmSolution.Instance["Flow1"];

        //    p_box.ModuleSource = p;
        //    p.Run();
        //}
        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "solution files (*.sol)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                TxtLogFilePath.Text = File.ReadAllText(openFileDialog.FileName);
        }

    }
}


