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
using X_Vision.Common.Models;
using X_Vision.Event;

namespace X_Vision.Views
{
    /// <summary>
    /// Interaction logic for ProcessModuleView.xaml
    /// </summary>
    public partial class ProcessModuleView : UserControl
    {          
        public ProcessModuleView(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.GetEvent<MessageEvent>().Subscribe(arg =>
            {
                MessageBox.Show($"Received: {arg}");
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VmSolution.Load("C:\\Users\\dumbchun\\source\\repos\\X-Guide\\pic.sol");
            VmFrontEnd.LoadFrontendSource();
        }
    }
}
