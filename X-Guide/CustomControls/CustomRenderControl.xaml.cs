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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VM.Core;
using VMControls.Interface;
using VMControls.WPF.Release;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomRenderControl.xaml
    /// </summary>
    public partial class CustomRenderControl : UserControl
    {

        public IVmModule Procedure
        {
            get { return (IVmModule)GetValue(ProcedureProperty); }
            set {
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

            var RenderControl = d as CustomRenderControl;
            try
            {
                RenderControl.r_control.ModuleSource = e.NewValue as IVmModule;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            RenderControl.loadingCircle.Visibility = Visibility.Collapsed;
            RenderControl.loadingCircle.IsRunning = false;
        }

      

        public CustomRenderControl()
        {
            InitializeComponent();
        }


    }
}
