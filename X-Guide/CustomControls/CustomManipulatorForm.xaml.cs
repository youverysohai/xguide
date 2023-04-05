using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CustomManipulatorForm.xaml
    /// </summary>
    public partial class CustomManipulatorForm : UserControl
    {
        public CustomManipulatorForm()
        {
            InitializeComponent();
        }

        //public string InputIp
        //{
        //    get { return (string)GetValue(InputIpProperty); }
        //    set { SetValue(InputIpProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for InputIp.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty InputIpProperty =
        //    DependencyProperty.Register("InputIp", typeof(string), typeof(CustomIpInput), new PropertyMetadata(0));

        //private static void OnIpInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    string inputIp = "";
        //    ObservableCollection<string> stringCollection = e.NewValue as ObservableCollection<string>;
        //    if (stringCollection != null)
        //    {
        //        inputIp = string.Join(" ", stringCollection);
        //        // Use the concatenated string as needed
        //    }
        //    if (string.IsNullOrEmpty(inputIp))
        //    {
        //        // Clear the text of all four TextBoxes if the input IP is null or empty.
        //        ((CustomIpInput)d).Ip0.Text = string.Empty;
        //        ((CustomIpInput)d).Ip1.Text = string.Empty;
        //        ((CustomIpInput)d).Ip2.Text = string.Empty;
        //        ((CustomIpInput)d).Ip3.Text = string.Empty;
        //    }
        //    else
        //    {
        //        // Split the input IP string by '.' and set the text of each TextBox accordingly.
        //        string[] ip = inputIp.Split('.');
        //        ((CustomIpInput)d).Ip0.Text = ip.Length > 0 ? ip[0] : string.Empty;
        //        ((CustomIpInput)d).Ip1.Text = ip.Length > 1 ? ip[1] : string.Empty;
        //        ((CustomIpInput)d).Ip2.Text = ip.Length > 2 ? ip[2] : string.Empty;
        //        ((CustomIpInput)d).Ip3.Text = ip.Length > 3 ? ip[3] : string.Empty;
        //    }
        //}



        //Purpose: limit the number of characters that can be entered 
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
        private void TxtPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length >= 5)
            {
                e.Handled = true; // cancel the input event
            }
        }

        //Purpose: if entered more than 3 text move to subsequent textbox
        private void TxtIPLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length == 3)
            {
                RequestFocusChange(FocusNavigationDirection.Next);
            }

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
