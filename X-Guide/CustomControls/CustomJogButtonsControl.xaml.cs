using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomJogButtonsControl.xaml
    /// </summary>
    public partial class CustomJogButtonsControl : UserControl
    {
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CustomJogButtonsControl), new PropertyMetadata(true));

        public bool IsHalfShifted
        {
            get { return (bool)GetValue(IsHalfShiftedProperty); }
            set { SetValue(IsHalfShiftedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHalfShifted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHalfShiftedProperty =
            DependencyProperty.Register("IsHalfShifted", typeof(bool), typeof(CustomJogButtonsControl), new PropertyMetadata(true));

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(CustomJogButtonsControl), new PropertyMetadata(null));

        public string TopCommandParameter
        {
            get { return (string)GetValue(TopCommandParameterProperty); }
            set { SetValue(TopCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopCommandParameterProperty =
            DependencyProperty.Register("TopCommandParameter", typeof(string), typeof(CustomJogButtonsControl), new PropertyMetadata(""));

        public string BottomCommandParameter
        {
            get { return (string)GetValue(BottomCommandParameterProperty); }
            set { SetValue(BottomCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomCommandParameterProperty =
            DependencyProperty.Register("BottomCommandParameter", typeof(string), typeof(CustomJogButtonsControl), new PropertyMetadata(""));

        public string LeftCommandParameter
        {
            get { return (string)GetValue(LeftCommandParameterProperty); }
            set { SetValue(LeftCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftCommandParameterProperty =
            DependencyProperty.Register("LeftCommandParameter", typeof(string), typeof(CustomJogButtonsControl), new PropertyMetadata(""));

        public string RightCommandParameter
        {
            get { return (string)GetValue(RightCommandParameterProperty); }
            set { SetValue(RightCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightCommandParameterProperty =
            DependencyProperty.Register("RightCommandParameter", typeof(string), typeof(CustomJogButtonsControl), new PropertyMetadata(""));

        public CustomJogButtonsControl()
        {
            InitializeComponent();
        }
    }
}