using IMVSCircleFindModuCs;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using VM.Core;
using VMControls.WPF.Release;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step6.xaml
    /// </summary>
    public partial class Step6 : UserControl
    {

  
        VmProcedure p;
/*        public ObservableCollection<VmRenderControl> Items { get; } = new ObservableCollection<VmRenderControl>();*/

        public Step6()
        {
            InitializeComponent();
           
     /*       Items.Add(new VmRenderControl());
            Items.Add(new VmRenderControl());
            Items.Add(new VmRenderControl());
            Items.Add(new VmRenderControl());
            Items.Add(new VmRenderControl());
            flipView1.ItemsSource = Items;*/

        }
        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                VmSolution.Import(@"C:\Users\Admin\Desktop\livecam.sol");

                //VmSolution.CreatSolInstance();
                p = (VmProcedure)VmSolution.Instance["CircleOut"];
                p.Run();
         /*       Items[0].ModuleSource = p;
                Items[1].ModuleSource = p;
                Items[2].ModuleSource = p;
                Items[3].ModuleSource = p;
                Items[4].ModuleSource = p;*/
                var circleFind = (IMVSCircleFindModuTool)VmSolution.Instance["CircleOut.Circle Search1"];
                circleFind.Run();
    /*            Items[0].ModuleSource = circleFind;
                Items[1].ModuleSource = circleFind;
                Items[2].ModuleSource = circleFind;
                Items[3].ModuleSource = circleFind;
                Items[4].ModuleSource = circleFind;*/
                //p_box.LoadFrontendSource();

                ////p_box.BindSingleProcedure(p.ToString());

                //p_box.AutoChangeSize();

            }
            catch
            {
          ;
            }

        }

 
     
    }
}
