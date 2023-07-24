using IMVSCircleFindModuCs;
using Prism.Events;
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
using VMControls.WPF.Release;
using X_Vision.Common.Models;
using X_Vision.Event;

namespace X_Vision.Views
{
    /// <summary>
    /// Interaction logic for ProcessModuleView.xaml
    /// </summary>
    public partial class ProcessModuleView : UserControl
    {
        VmProcedure vmProcedure;

        public ProcessModuleView(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.GetEvent<MessageEvent>().Subscribe(arg =>
            {
                MessageBox.Show($"Received: {arg}");
            });
        }
        private void RunOnce_Click(object sender, RoutedEventArgs e)
        {
            IMVSCircleFindModuTool circleTool = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            vmProcedure.Run();
            var i = circleTool.ModuResult;

            //renderControl.ModuleSource = vmProcedure;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            VmSolution.Load(@"C:\Users\tung\Downloads\findCircle.sol");
            vmProcedure = (VmProcedure)VmSolution.Instance["Flow1"];
            
            IMVSCircleFindModuTool circleTool = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            vmProcedure.Run();
            var i = circleTool.ModuResult;
            
           // renderControl.ModuleSource = vmProcedure;
        }
        private void ContinueRun_Click(object sender, RoutedEventArgs e)
        {
            vmProcedure.ContinuousRunEnable = true;

           // renderControl.ModuleSource = vmProcedure;
        } 
        private void Stop_Click(object sender, RoutedEventArgs e)
        {

 
            vmProcedure.ContinuousRunEnable = false;
            //renderControl.ModuleSource = vmProcedure;
        }
    }
}
