using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5TopConfig.xaml
    /// </summary>
    public partial class Step5TopConfig : UserControl
    {
        public ICommand JogCommand
        {
            get { return (ICommand)GetValue(JogCommandProperty); }
            set { SetValue(JogCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JogCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JogCommandProperty =
            DependencyProperty.Register("JogCommand", typeof(ICommand), typeof(Step5TopConfig), new PropertyMetadata(null));

        public Step5TopConfig()
        {
            InitializeComponent();
        }
    }
}