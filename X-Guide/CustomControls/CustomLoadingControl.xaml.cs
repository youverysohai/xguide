using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomLoadingControl.xaml
    /// </summary>
    public partial class CustomLoadingControl : UserControl
    {
        public DialogHost dialogHost { get; set; }

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRunning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(CustomLoadingControl), new PropertyMetadata(true, OnLoadingChanged));

        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
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

            if (custControl.IsRunning && !(custControl.dialogHost is null))
            {
                await Application.Current.Dispatcher.BeginInvoke(() => custControl.dialogHost.ShowDialog(custControl.DialogContent));
            }
            else
            {
                await Task.Delay(100);
                custControl.dialogHost.IsOpen = false;
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