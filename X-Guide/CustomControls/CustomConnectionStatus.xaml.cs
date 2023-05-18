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
    }
}
