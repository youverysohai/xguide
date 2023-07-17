using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using X_Guide.Enums;
using ManipulatorType = XGuideSQLiteDB.Models.ManipulatorType;
using Orientation = X_Guide.Enums.Orientation;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for OrientationGroup.xaml
    /// </summary>
    [INotifyPropertyChanged]
    public partial class OrientationGroup : UserControl
    {
        public static List<ValueDescription> AllOrientations { get; set; } = EnumHelperClass.GetAllIntAndDescriptions(typeof(Orientation)).ToList();

        public ObservableCollection<ValueDescription> Orientations { get; set; } = new ObservableCollection<ValueDescription>(AllOrientations.GetRange(0, 3));

        public ManipulatorType ManipulatorType
        {
            get { return (ManipulatorType)GetValue(ManipulatorTypeProperty); }
            set { SetValue(ManipulatorTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ManipulatorType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ManipulatorTypeProperty =
            DependencyProperty.Register("ManipulatorType", typeof(ManipulatorType), typeof(OrientationGroup), new PropertyMetadata(ManipulatorType.GantrySystemR, OnManipulatorTypeChanged));

        private static void OnManipulatorTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is OrientationGroup oGroup)
            {
                var oList = AllOrientations.ToList();

                switch (e.NewValue)
                {
                    case ManipulatorType.GantrySystemWR: oList = AllOrientations.GetRange(0, 3); break;
                    case ManipulatorType.SCARA: oList.RemoveAt(4); break;
                    case ManipulatorType.SixAxis: oList.RemoveAt(3); break;
                }

                oGroup.Orientations = new ObservableCollection<ValueDescription>(oList);
            }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(OrientationGroup), new FrameworkPropertyMetadata(Orientation.EyeOnHand, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public OrientationGroup()
        {
            InitializeComponent();
        }
    }

    public class OrientationEnumConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Orientation)value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}