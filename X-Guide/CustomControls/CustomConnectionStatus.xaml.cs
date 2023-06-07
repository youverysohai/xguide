using System.Windows;
using System.Windows.Controls;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomConnectionStatus.xaml
    /// </summary>
    public partial class CustomConnectionStatus : UserControl
    {
        public CustomConnectionStatus()
        {
            InitializeComponent();
        }

        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnected.  This enables animation, styling, binding, etc...
        //propertyMetadata default value
        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(CustomConnectionStatus), new PropertyMetadata(false));

        public Visibility ShowTitle
        {
            get { return (Visibility)GetValue(IsMainWindowProperty); }
            set { SetValue(IsMainWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMainWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMainWindowProperty =
            DependencyProperty.Register("IsMainWindow", typeof(Visibility), typeof(CustomConnectionStatus), new PropertyMetadata(Visibility.Visible));
    }
}