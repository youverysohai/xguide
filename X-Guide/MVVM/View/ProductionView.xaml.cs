using System.Windows;
using System.Windows.Controls;
using VM.Core;

//using VM.Core;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class ProductionView : UserControl
    {
        private VmProcedure p;

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
                VmSolution.Load(@"C:\Users\Xlent-Tung\Desktop\livecam.sol", "");
                p = (VmProcedure)VmSolution.Instance["Flow1"];
                p.Run();
                p_box.LoadFrontendSource();
                //p_box.BindSingleProcedure(p.ToString());
                p_box.AutoChangeSize();
            }
            catch
            {
            }
        }
    }
}