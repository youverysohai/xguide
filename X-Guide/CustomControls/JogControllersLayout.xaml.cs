using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        //        private readonly static RadialMenu[] menuBars = new MenuBar[]
        //{
        //            new MenuBar() { Icon = "HomeOutline", Title = "Process", NameSpace = "ProcessModuleView" },
        //            new MenuBar() { Icon = "CameraOutline", Title = "Camera", NameSpace = "CameraModuleView" }             ,
        //            new MenuBar() { Icon = "Connection", Title = "Communication", NameSpace = "CommunicationModuleView" },
        //            new MenuBar() { Icon = "WrenchCogOutline", Title = "Advanced Settings", NameSpace = "SettingsModuleView" },
        //            new MenuBar() { Icon = "FileCogOutline", Title = "Log File", NameSpace = "LogFileModuleView" }
        //};

        //private readonly List<RadialMenuItem> MainMenuItems = new List<RadialMenuItem>
        //    {

        //        new RadialMenuItem
        //        {
        //            Content = new TextBlock { Text = "X+" },
        //            ArrowBackground = Brushes.Transparent,
        //            CommandParameter="X+"
        //            Command = new 

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
            DependencyProperty.Register("Type", typeof(ManipulatorType), typeof(JogControllersLayout), new PropertyMetadata(ManipulatorType.SixAxis, TypeChanged));

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ManipulatorType type)
            {
                JogControllersLayout jogLayout = (JogControllersLayout)d;
                //CustomJogButtonsControl sixAxis = jogLayout.FindName("SixAxis") as CustomJogButtonsControl;
                //CustomJogButtonsControl grscara = jogLayout.FindName("GRSCARA") as CustomJogButtonsControl;
                // var rd = jogLayout.FindName("RadialMenu") as RadialMenu;

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
            Step5ViewModel vm = new Step5ViewModel();
             
            var MainMenuItems = new List<RadialMenuItem>
            {
                new RadialMenuItem
                {
                    Content = new TextBlock { Text = "Y+" }, 
                    Command = vm.TestCommand

                },
                new RadialMenuItem
                {
                    Content = new TextBlock { Text = "X+" }, 
                    Command = vm.TestCommand
                },
                new RadialMenuItem
                {
                    Command = vm.TestCommand,
                    Content = new TextBlock { Text = "Y-" ,
                    }
                },
                new RadialMenuItem
                {
                    Command = vm.TestCommand,
                    Content = new TextBlock { Text = "X-" ,
                    }
                }
            };
            RadialMenu.Items = MainMenuItems;
        }


    }
}
