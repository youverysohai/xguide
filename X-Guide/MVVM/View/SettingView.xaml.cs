
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Serialization;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {

        //ILoggerFactory loggerFactory;
        //ILogger logger;
      
        public SettingView()
        {
            InitializeComponent();
            //loggerFactory = new LoggerFactory();
            //loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logs"));
            //logger = loggerFactory.CreateLogger<MainWindow>();
            //logger.LogDebug("Configuration loaded.");
            //logger.LogDebug("User login into system.");
        }

     

        //Purpose: limit the number of characters that can be entered 
        private void TxtIPAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length >= 3)
            {
                e.Handled = true; // cancel the input event
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

            //if (txtRobotIP1.Text.Length == 3)
            //{
            //    txtRobotIP2.Focus();
            //    if (txtRobotIP2.Text.Length == 3)
            //    {
            //        txtRobotIP3.Focus();

            //        if (txtRobotIP3.Text.Length == 3)
            //        {
            //            txtRobotIP4.Focus();
            //        }
            //    }
            //}
        }
        private void TxtScannerIPLength_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtScannerIP1.Text.Length == 3)
            {
                txtScannerIP2.Focus();
                if (txtScannerIP2.Text.Length == 3)
                {
                    txtScannerIP3.Focus();
                    if (txtScannerIP3.Text.Length == 3)
                    {
                        txtScannerIP4.Focus();
                    }
                }
            }

        }

        private void txtRobotIP2_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.Key == System.Windows.Input.Key.Back)
            {
                e.Handled = true;
                if (tb.Text.Length == 0)
                {
                    RequestFocusChange(FocusNavigationDirection.Previous);
                }
            }
        }

        private static bool RequestFocusChange(FocusNavigationDirection direction)
        {
            var focused = Keyboard.FocusedElement as UIElement;
            if (focused != null) return focused.MoveFocus(new TraversalRequest(direction));
            return false;
        }

        private void txtRobotIP1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                RequestFocusChange(FocusNavigationDirection.Next);
            }
        }
    }
}
