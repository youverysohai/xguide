using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for CustomVisionForm.xaml
    /// </summary>
    public partial class CustomVisionForm : UserControl
    {
        public CustomVisionForm()
        {
            InitializeComponent();
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

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "sol files (*.sol)|*.sol|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
                VisionPathTextBox.Text = File.ReadAllText(openFileDialog.FileName);
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
