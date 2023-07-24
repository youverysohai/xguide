using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;
using VMControls.WPF;
using VmRenderControl = VMControls.WPF.Release.VmRenderControl;

namespace X_Vision.Common.CustomUserControl
{
    /// <summary>
    /// Interaction logic for CustomRenderControl.xaml
    /// </summary>
    public partial class CustomRenderControl : UserControl, IDisposable
    {
        public static Dictionary<int, VmRenderControl> controls;

        public Visibility CenterBorder
        {
            get { return (Visibility)GetValue(CenterBorderProperty); }
            set { SetValue(CenterBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterBorderProperty =
            DependencyProperty.Register("CenterBorder", typeof(Visibility), typeof(CustomRenderControl), new PropertyMetadata(Visibility.Collapsed, OnCenterBorderChanged));

        private static void OnCenterBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomRenderControl custRenderControl)
            {
                var renderControl = custRenderControl.r_control;

                if (e.NewValue is Visibility.Visible)
                {
                    custRenderControl.DrawShape(default);
                    custRenderControl.Subscribe();
                }
                else
                {
                    custRenderControl.Unsubscribe();
                }
            }
        }

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

        public static event EventHandler<VmRenderControl> OnRenderControlChanged;

        // Using a DependencyProperty as the backing store for Procedure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcedureProperty =
            DependencyProperty.Register("Procedure", typeof(IVmModule), typeof(CustomRenderControl), new PropertyMetadata(null, OnProcedureChanged));

        private static void OnProcedureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var custRenderControl = d as CustomRenderControl;

            custRenderControl.Unsubscribe();

            var renderControl = custRenderControl.r_control;

            renderControl.ImageSource = null;

            renderControl.ModuleSource = e.NewValue as IVmModule;

            custRenderControl.loadingCircle.Visibility = Visibility.Collapsed;
            custRenderControl.loadingCircle.IsRunning = false;
        }

        private void Subscribe()
        {
            VmSolution.OnWorkStatusEvent += DrawShape;
        }

        private void Unsubscribe()
        {
            VmSolution.OnWorkStatusEvent -= DrawShape;
        }

        private void DrawShape(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            var ImageSource = r_control.ImageSource;
            if (ImageSource is null) return;
            var centerPoint = new System.Windows.Point(ImageSource.Width / 2, ImageSource.Height / 2);
            //double width = ImageSource.Width / 3;
            //double height = ImageSource.Height / 3;
            //RectangleEx centerRectangle = new RectangleEx(centerPoint, width, height);
            //r_control.AddShape(centerRectangle);
        }

        public CustomRenderControl()
        {
            InitializeComponent();
            Debug.WriteLine("Load once");
            //VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;
        }

        public void Dispose()
        {
            VmSolution.OnWorkStatusEvent -= DrawShape;
        }
    }
}