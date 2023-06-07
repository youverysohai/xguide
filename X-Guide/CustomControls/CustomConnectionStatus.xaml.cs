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
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(CustomConnectionStatus), new PropertyMetadata(false, OnConnectedStatusChanged));

        private static void OnConnectedStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CustomConnectionStatus customConnectionStatus)) return;

            var i = customConnectionStatus.IsConnected;
            
        }



        public bool IsVisionConnected
        {
            get { return (bool)GetValue(IsVisionConnectedProperty); }
            set { SetValue(IsVisionConnectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisionConnected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisionConnectedProperty =
            DependencyProperty.Register("IsVisionConnected", typeof(bool), typeof(CustomConnectionStatus), new PropertyMetadata(false,OnVisionConnectionStatusChanged));

        private static void OnVisionConnectionStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CustomConnectionStatus customConnectionStatus)) return;

            var i = customConnectionStatus.IsVisionConnected;
        }

        public Visibility ShowVisionStatus
        {
            get { return (Visibility)GetValue(ShowVisionStatusProperty); }
            set { SetValue(ShowVisionStatusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ShowVisionStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowVisionStatusProperty =
            DependencyProperty.Register("ShowVisionStatus", typeof(Visibility), typeof(CustomConnectionStatus), new PropertyMetadata(Visibility.Collapsed));

        public Visibility IsMainWindow
        {
            get { return (Visibility)GetValue(IsMainWindowProperty); }
            set { SetValue(IsMainWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMainWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMainWindowProperty =
            DependencyProperty.Register("IsMainWindow", typeof(Visibility), typeof(CustomConnectionStatus), new PropertyMetadata(Visibility.Visible));


    }
}
