using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using X_Guide.MVVM.ViewModel;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomManipulatorForm.xaml
    /// </summary>
    public partial class CustomManipulatorForm : UserControl
    {
        public CustomManipulatorForm()
        {
            InitializeComponent();
        }
        public ManipulatorViewModel Manipulator
        {
            get { return (ManipulatorViewModel)GetValue(ManipulatorProperty); }
            set { SetValue(ManipulatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Manipulator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ManipulatorProperty =
            DependencyProperty.Register("Manipulator", typeof(ManipulatorViewModel), typeof(CustomManipulatorForm), new PropertyMetadata(null));

     
    }
}
