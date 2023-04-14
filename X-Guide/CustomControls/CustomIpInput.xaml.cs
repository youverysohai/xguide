using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for CustomIpInput.xaml
    /// </summary>
    /// 
    [AddINotifyPropertyChangedInterface]
    public partial class CustomIpInput : UserControl, INotifyPropertyChanged
    {


        private volatile bool _updating = false;
        public string Ip
        {
            get { return (string)GetValue(IpSegmentProperty); }
            set { SetValue(IpSegmentProperty, value); }
        }

        public string T1 { get; set; }
        public string T2 { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }

        // Using a DependencyProperty as the backing store for IpSegment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IpSegmentProperty =
            DependencyProperty.Register("Ip", typeof(string), typeof(CustomIpInput), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IpChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void IpChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var c = d as CustomIpInput;
            if (c._updating) return;
            List<string> segments = c.Ip?.Split('.').ToList() ?? new List<string>();

            c.UpdateTextbox(segments);


        }

        public CustomIpInput()
        {
            InitializeComponent();
        }


        private void UpdateTextbox(List<string> segments)
        {
            _updating = true;
            T1 = segments.ElementAtOrDefault(0);
            T2 = segments.ElementAtOrDefault(1);
            T3 = segments.ElementAtOrDefault(2);
            T4 = segments.ElementAtOrDefault(3);
            _updating = false;
        }


        private void TxtIPAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length >= 3)
            {
                if (textBox.SelectionLength == textBox.Text.Length)
                {
                    e.Handled = false; // do not cancel the input event
                }
                else
                {
                    e.Handled = true; // cancel the input event
                }

            }

        }


        //Purpose: if entered more than 3 text move to subsequent textbox
        private void TxtIPLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating) return;
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length == 3)
            {
                RequestFocusChange(FocusNavigationDirection.Next);
            }

            _updating = true;
            Ip = string.Join(".", new string[] { T1, T2, T3, T4 });
            _updating = false;
        }

        private static bool RequestFocusChange(FocusNavigationDirection direction)
        {
            var focused = Keyboard.FocusedElement as UIElement;
            if (focused != null) return focused.MoveFocus(new TraversalRequest(direction));
            return false;
        }

        private void txtIP_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                RequestFocusChange(FocusNavigationDirection.Next);
            }
            if (e.Key == Key.Right && tb.CaretIndex == tb.Text.Length)
            {
                e.Handled = true;
                RequestFocusChange(FocusNavigationDirection.Next);
            }
            if (e.Key == Key.Back && tb.Text.Length == 0 || e.Key == Key.Back && tb.CaretIndex == 0 && tb.Text.Length == 0)
            {
                e.Handled = true;
                RequestFocusChange(FocusNavigationDirection.Previous);

            }
            if (e.Key == Key.Left && tb.CaretIndex == 0)
            {
                e.Handled = true;
                RequestFocusChange(FocusNavigationDirection.Previous);
            }
        }
        private void txtIP_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

    }
}
