using System.Windows;
using System.Windows.Controls;
using XGuideSQLiteDB.Models;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for ManipulatorComboBox.xaml
    /// </summary>
    public partial class ManipulatorComboBox : UserControl
    {
        public ManipulatorType Type
        {
            get { return (ManipulatorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ManipulatorType), typeof(ManipulatorComboBox), new FrameworkPropertyMetadata(ManipulatorType.GantrySystemR, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ManipulatorComboBox()
        {
            InitializeComponent();
        }
    }
}