using CalibrationProvider;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Guide.CustomControls.Layouts
{
    /// <summary>
    /// Interaction logic for CalibrationResultLayout.xaml
    /// </summary>
    ///

    public partial class CalibrationResultLayout : UserControl
    {
        public ICommand SaveCalibrationCommand
        {
            get { return (ICommand)GetValue(SaveCalibrationCommandProperty); }
            set { SetValue(SaveCalibrationCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveCalibrationCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveCalibrationCommandProperty =
            DependencyProperty.Register("SaveCalibrationCommand", typeof(ICommand), typeof(CalibrationResultLayout), new PropertyMetadata(null));



        public ICommand StartCalibrationCommand
        {
            get { return (ICommand)GetValue(StartCalibrationCommandProperty); }
            set { SetValue(StartCalibrationCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartCalibrationCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartCalibrationCommandProperty =
            DependencyProperty.Register("StartCalibrationCommand", typeof(ICommand), typeof(CalibrationResultLayout), new PropertyMetadata(null));



        public CalibrationData CalibrationData
        {
            get { return (CalibrationData)GetValue(CalibrationProperty); }
            set { SetValue(CalibrationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalibrationProperty =
            DependencyProperty.Register("CalibrationData", typeof(CalibrationData), typeof(CalibrationResultLayout), new PropertyMetadata(null));

        public CalibrationResultLayout()
        {
            InitializeComponent();
        }
    }
}