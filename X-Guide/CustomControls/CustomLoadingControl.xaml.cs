using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using VMControls.Interface;
using Windows.UI.Xaml.Controls.Maps;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomLoadingControl.xaml
    /// </summary>
    public partial class CustomLoadingControl : UserControl
    {
        public DialogHost dialogHost { get; set; }


        public bool testing
        {
            get { return (bool)GetValue(testingProperty); }
            set { SetValue(testingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for testing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty testingProperty =
            DependencyProperty.Register("testing", typeof(bool), typeof(CustomLoadingControl), new PropertyMetadata(false, OnLoadingChanged));





        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(CustomLoadingControl), new PropertyMetadata(null));


        public UserControl ParentControl
        {
            get { return (UserControl)GetValue(ParentControlProperty); }
            set { SetValue(ParentControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentControlProperty =
            DependencyProperty.Register("ParentControl", typeof(object), typeof(CustomLoadingControl), new PropertyMetadata(null, OnParentControlChanged));


        private static async void OnLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var custControl = (d as CustomLoadingControl);
            if (custControl.testing && !(custControl.dialogHost is null))
            {
                Application.Current.Dispatcher.BeginInvoke(() => custControl.dialogHost.ShowDialog(custControl.DialogContent));

                custControl.dialogHost.IsOpen = false;
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(() => custControl.dialogHost.IsOpen = false);
            }

        }

        private static void OnParentControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


            var custControl = (d as CustomLoadingControl);
            ContentControl parentControl = (e.NewValue as ContentControl);
            var stackPanel = new Grid();
            stackPanel.Children.Add(new ContentControl()
            {
                Content = parentControl.Content,
            });
            custControl.dialogHost = new DialogHost();
            custControl.dialogHost.Content = stackPanel;
            parentControl.Content = custControl.dialogHost;


        }



        public CustomLoadingControl()
        {
            var i = new StackPanel();

            InitializeComponent();
        }
    }
}
