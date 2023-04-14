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

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for ManipulatorComboBox.xaml
    /// </summary>
    public partial class ManipulatorComboBox : UserControl
    {



        public int Type
        {
            get { return (int)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(ManipulatorComboBox), new FrameworkPropertyMetadata(-1,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ManipulatorComboBox()
        {
            InitializeComponent();
        }
    }
}
