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
        public bool isStarted { get; set; } = false;
        public ICommand JogCommand
        {
            get { return (ICommand)GetValue(JogCommandProperty); }
            set { SetValue(JogCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JogCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JogCommandProperty =
            DependencyProperty.Register("JogCommand", typeof(ICommand), typeof(Step5TopConfig), new PropertyMetadata(null));

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            isStarted = true;
        }
    }
}