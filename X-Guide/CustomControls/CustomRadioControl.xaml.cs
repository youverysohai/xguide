using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using X_Guide.Enums;
using Orientation = X_Guide.Enums.Orientation;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomRadioControl.xaml
    /// </summary>
    public partial class CustomRadioControl : UserControl
    {
        public IEnumerable<ValueDescription> ItemSource
        {
            get { return (IEnumerable<ValueDescription>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(IEnumerable<ValueDescription>), typeof(CustomRadioControl), new PropertyMetadata(null));

        public Orientation SelectedValue
        {
            get { return (Orientation)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(CustomRadioControl), new FrameworkPropertyMetadata(Orientation.EyeOnHand, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedPropertiesChanged));

        private static void OnSelectedPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as CustomRadioControl;
        }

        public CustomRadioControl()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                object tag = radioButton.Tag;
                SelectedValue = (Orientation)tag;
            }
        }
    }
}