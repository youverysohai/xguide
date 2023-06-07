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
    /// Interaction logic for CustomFormButtons.xaml
    /// </summary>
    public partial class CustomFormButtons : UserControl
    {


        public Visibility ShowSaveDeleteBtn
        {
            get { return (Visibility)GetValue(ShowSaveDeleteBtnProperty); }
            set { SetValue(ShowSaveDeleteBtnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSaveDeleteBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSaveDeleteBtnProperty =
            DependencyProperty.Register("ShowSaveDeleteBtn", typeof(Visibility), typeof(CustomFormButtons), new PropertyMetadata(Visibility.Visible));




        public ICommand AddCommand 
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommand .  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(CustomFormButtons), new PropertyMetadata(null));



        public object AddCommandParameter
        {
            get { return (object)GetValue(AddCommandParameterProperty); }
            set { SetValue(AddCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommandParameterProperty =
            DependencyProperty.Register("AddCommandParameter", typeof(object), typeof(CustomFormButtons), new PropertyMetadata(null));



        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(CustomFormButtons), new PropertyMetadata(null));




        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(CustomFormButtons), new PropertyMetadata(null));




        public CustomFormButtons()
        {
            InitializeComponent();
        }
    }
}
