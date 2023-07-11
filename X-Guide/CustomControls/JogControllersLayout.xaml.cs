using System;
using System.Collections.Generic;
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
using X_Guide.Enums;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for JogControllersLayout.xaml
    /// </summary>
    public partial class JogControllersLayout : UserControl
    {



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
            if(e.NewValue is ManipulatorType type)
            {
                JogControllersLayout jogLayout = (JogControllersLayout)d;
                CustomJogButtonsControl sixAxis =jogLayout.FindName("SixAxis") as CustomJogButtonsControl;
                CustomJogButtonsControl grscara =jogLayout.FindName("GRSCARA") as CustomJogButtonsControl;
                
                switch (type)
                {
                    case ManipulatorType.GantrySystemWR: sixAxis.IsOpen = false; grscara.IsOpen = false; break;
                    case ManipulatorType.GantrySystemR: sixAxis.IsOpen = false; break;
                    case ManipulatorType.SCARA: sixAxis.IsOpen = false; break;
                    case ManipulatorType.SixAxis: sixAxis.IsOpen = false; break;
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
