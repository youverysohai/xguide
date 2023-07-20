using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Guide.CustomControls.Layouts
{
    /// <summary>
    /// Interaction logic for XYDistanceLayout.xaml
    /// </summary>
    public partial class JogTrackingLayout : UserControl
    {


        public int XDistance
        {
            get { return (int)GetValue(XDistanceProperty); }
            set { SetValue(XDistanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XDistanceProperty =
            DependencyProperty.Register("XDistance", typeof(int), typeof(JogTrackingLayout), new PropertyMetadata(0));

        public int YDistance
        {
            get { return (int)GetValue(YDistanceProperty); }
            set { SetValue(YDistanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YDistanceProperty =
            DependencyProperty.Register("YDistance", typeof(int), typeof(JogTrackingLayout), new PropertyMetadata(0));



        public ICommand StartJogTrackingCommand
        {
            get { return (ICommand)GetValue(StartJogTrackingCommandProperty); }
            set { SetValue(StartJogTrackingCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartJogTrackingCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartJogTrackingCommandProperty =
            DependencyProperty.Register("StartJogTrackingCommand", typeof(ICommand), typeof(JogTrackingLayout), new PropertyMetadata(null));





        public JogTrackingLayout()
        {
            InitializeComponent();
        }

    }
}