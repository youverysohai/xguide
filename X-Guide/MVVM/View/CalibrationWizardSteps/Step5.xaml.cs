﻿
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using VM.Core;
using VM.PlatformSDKCS;
using System.Diagnostics;
using VMControls.Winform.Release;
using IMVSCircleFindModuCs;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5.xaml
    /// </summary>
    /// 

    public partial class Step5 : UserControl
    {
        VmProcedure p;


        public Step5()
        {
            InitializeComponent();


        }
        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                VmSolution.Import(@"C:\Users\Admin\Desktop\livecam.sol");

                //VmSolution.CreatSolInstance();
                p = (VmProcedure)VmSolution.Instance["CircleOut"];
                p.Run();
                p_box.ModuleSource = p;
                var circleFind = (IMVSCircleFindModuTool)VmSolution.Instance["CircleOut.Circle Search1"];
                circleFind.Run();
                p_box.ModuleSource = circleFind;
                //p_box.LoadFrontendSource();
                
                ////p_box.BindSingleProcedure(p.ToString());
                
                //p_box.AutoChangeSize();

            }
            catch
            {
                Debug.WriteLine("Everything is fine");
            }

        }

        private void p_box_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {

                //p_box.LoadFrontendSource();
                ////p_box.BindSingleProcedure(p.ToString());
                //p_box.AutoChangeSize();

            }
            catch
            {
                Debug.WriteLine("Everything is fine");
            }

        }
    }


}
