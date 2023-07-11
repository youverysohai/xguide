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
using X_Guide.CustomControls;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5TopConfig.xaml
    /// </summary>
    public partial class Step5TopConfig : UserControl
    {


        public CalibrationViewModel Calibration
        {
            get { return (CalibrationViewModel)GetValue(CalibrationProperty); }
            set { SetValue(CalibrationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Calibration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalibrationProperty =
            DependencyProperty.Register("Calibration", typeof(CalibrationViewModel), typeof(Step5TopConfig), new PropertyMetadata(null));


        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(Step5TopConfig), new PropertyMetadata(null));

        public Step5TopConfig()
        {
            InitializeComponent();
        }
    }
}
