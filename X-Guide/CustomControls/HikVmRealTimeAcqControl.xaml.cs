using System.Windows;
using System.Windows.Controls;
using VM.Core;
using VMControls.Interface;
using VMControls.WPF.Release;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for HikVmRealTimeAcqControl.xaml
    /// </summary>
    public partial class HikVmRealTimeAcqControl : UserControl
    {
        public HikVmRealTimeAcqControl()
        {
            InitializeComponent();
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

        // Using a DependencyProperty as the backing store for Procedure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcedureProperty =
            DependencyProperty.Register("Procedure", typeof(IVmModule), typeof(HikVmRealTimeAcqControl), new PropertyMetadata(null, OnProcedureChanged));

        private static void OnProcedureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HikVmRealTimeAcqControl owner = d as HikVmRealTimeAcqControl;
            VmRealTimeAcqControl acqControl = owner.acq_control;
            acqControl.ModuleSource = e.NewValue as VmModule;
            //i.ModuleResultCallBackArrived += (s, args) => testing(width, height, renderControl);
            //RenderControl.r_control.UpdateVMResultShow();
        }
    }
}