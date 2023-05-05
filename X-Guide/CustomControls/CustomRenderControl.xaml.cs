using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VM.Core;
using VMControls.Interface;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomRenderControl.xaml
    /// </summary>
    public partial class CustomRenderControl : UserControl
    {
        public Visibility CenterBorder
        {
            get { return (Visibility)GetValue(CenterBorderProperty); }
            set { SetValue(CenterBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterBorderProperty =
            DependencyProperty.Register("CenterBorder", typeof(Visibility), typeof(CustomRenderControl), new PropertyMetadata(Visibility.Collapsed));

        public IVmModule Procedure
        {
            get { return (IVmModule)GetValue(ProcedureProperty); }
            set
            {
                if (value is IVmModule)
                {
                    SetValue(ProcedureProperty, value);
                }
            }
        }

        // Using a DependencyProperty as the backing store for Procedure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcedureProperty =
            DependencyProperty.Register("Procedure", typeof(IVmModule), typeof(CustomRenderControl), new PropertyMetadata(null, OnProcedureChanged));

        private static void OnProcedureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var custRenderControl = d as CustomRenderControl;

            var renderControl = custRenderControl.r_control;

            renderControl.IsShowCustomROIMenu = true;
            renderControl.ModuleSource = e.NewValue as IVmModule;

            var i = e.NewValue as VmModule;
            //double height = renderControl.ImageSource.Height / 2;
            //double width = renderControl.ImageSource.Width / 2;
            //i.ModuleResultCallBackArrived += (s, args) => testing(width, height, renderControl);
            //RenderControl.r_control.UpdateVMResultShow();
            custRenderControl.loadingCircle.Visibility = Visibility.Collapsed;
            custRenderControl.loadingCircle.IsRunning = false;
        }

        private static void trya(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("hiya");
        }

        public CustomRenderControl()
        {
            InitializeComponent();
        }

        private void r_control_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
        }

        private void r_control_Initialized(object sender, EventArgs e)
        {
        }

        private void r_control_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
        }
    }
}