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
    /// Interaction logic for CustLeftDrawer.xaml
    /// </summary>
    public partial class CustomLeftDrawer : UserControl
    {





        public object PContent
        {
            get { return (object)GetValue(PContentProperty); }
            set { SetValue(PContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PContentProperty =
            DependencyProperty.Register("PContent", typeof(object), typeof(CustomLeftDrawer), new PropertyMetadata(null));




        public IEnumerable<object> Items
        {
            get { return (IEnumerable<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<object>), typeof(CustomLeftDrawer), new PropertyMetadata(null));




        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomLeftDrawer), new PropertyMetadata(null));






        public CustomLeftDrawer()
        {
            InitializeComponent();
        }
    }
}
