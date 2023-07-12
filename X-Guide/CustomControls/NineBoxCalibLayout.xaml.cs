using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for NineBoxCalibLayout.xaml
    /// </summary>
    public partial class NineBoxCalibLayout : UserControl
    {
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(NineBoxCalibLayout), new PropertyMetadata("Ipsum Losem"));

        public ICommand StartCommand
        {
            get { return (ICommand)GetValue(StartCommandProperty); }
            set { SetValue(StartCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartCommandProperty =
            DependencyProperty.Register("StartCommand", typeof(ICommand), typeof(NineBoxCalibLayout), new PropertyMetadata(null));

        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextCommandProperty =
            DependencyProperty.Register("NextCommand", typeof(ICommand), typeof(NineBoxCalibLayout), new PropertyMetadata(null));

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(NineBoxCalibLayout), new PropertyMetadata(null));

        public ObservableCollection<bool> NinePointState
        {
            get { return (ObservableCollection<bool>)GetValue(NinePointStateProperty); }
            set { SetValue(NinePointStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NinePointState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NinePointStateProperty =
            DependencyProperty.Register("NinePointState", typeof(ObservableCollection<bool>), typeof(NineBoxCalibLayout), new FrameworkPropertyMetadata(new ObservableCollection<bool>(new bool[9])));

        public NineBoxCalibLayout()
        {
            InitializeComponent();
        }
    }
}