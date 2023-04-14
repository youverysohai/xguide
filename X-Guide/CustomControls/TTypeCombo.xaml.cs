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
    /// Interaction logic for TTypeCombo.xaml
    /// </summary>
    public partial class TTypeCombo : UserControl
    {
        public TTypeCombo()
        {
            InitializeComponent();
        }



        public string Terminator
        {
            get { return (string)GetValue(TerminatorProperty); }
            set { SetValue(TerminatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Terminator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TerminatorProperty =
            DependencyProperty.Register("Terminator", typeof(string), typeof(TTypeCombo), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    }
}
