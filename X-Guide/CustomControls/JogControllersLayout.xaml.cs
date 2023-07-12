using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using X_Guide.Enums;
using X_Guide.MVVM.View.CalibrationWizardSteps;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for JogControllersLayout.xaml
    /// </summary>
    public partial class JogControllersLayout : UserControl
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(JogControllersLayout), new PropertyMetadata(null));

        //        },
        //        new RadialMenuItem
        //        {
        //            Content = new TextBlock { Text = "X-" },
        //            ArrowBackground = Brushes.Transparent
        //        },
        //        new RadialMenuItem
        //        {

        //            Content = new TextBlock { Text = "Y+" ,

        //            }
        //        }
        //    };
        public ManipulatorType Type
        {
            get { return (ManipulatorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ManipulatorType), typeof(JogControllersLayout), new FrameworkPropertyMetadata(ManipulatorType.SixAxis, TypeChanged));

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ManipulatorType type)
            {
                JogControllersLayout jogLayout = d as JogControllersLayout;
                var sixAxis = jogLayout.SixAxis;
                var grscara = jogLayout.GRSCARA;

                switch (type)
                {
                    case ManipulatorType.GantrySystemWR:
                        //sixAxis.IsOpen = false;
                        //grscara.IsOpen = false;

                        break;
                    case ManipulatorType.GantrySystemR:  break;
                    case ManipulatorType.SCARA:  break;
                    case ManipulatorType.SixAxis: break;
                }
            }
        }

        public JogControllersLayout()
        {
            InitializeComponent();
        }

        public static void jogVisibility()
        {
        }


    }
}