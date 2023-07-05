using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
using Color = System.Windows.Media.Color;

namespace X_Vision.CustomUserControls
{
    /// <summary>
    /// Interaction logic for RateLayout.xaml
    /// </summary>
    public partial class RateLayout : UserControl
    {
        public RateLayout()
        {
            InitializeComponent();
        }



        public bool IsPassRate
        {
            get { return (bool)GetValue(IsPassRateProperty); }
            set { SetValue(IsPassRateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPassRate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPassRateProperty =
            DependencyProperty.Register("IsPassRate", typeof(bool), typeof(RateLayout), new PropertyMetadata(false));


        public int PercentageValue
        {
            get { return (int)GetValue(PercentageValueProperty); }
            set { SetValue(PercentageValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PercentageValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageValueProperty =
            DependencyProperty.Register("PercentageValue", typeof(int), typeof(RateLayout), new PropertyMetadata(0));



        public int TotalUnit
        {
            get { return (int)GetValue(TotalUnitProperty); }
            set { SetValue(TotalUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalUnitProperty =
            DependencyProperty.Register("TotalUnit", typeof(int), typeof(RateLayout), new PropertyMetadata(0));




        public int RateCount
        {
            get { return (int)GetValue(RateCountProperty); }
            set { SetValue(RateCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RateCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RateCountProperty =
            DependencyProperty.Register("RateCount", typeof(int), typeof(RateLayout), new PropertyMetadata(0));


    }




}
