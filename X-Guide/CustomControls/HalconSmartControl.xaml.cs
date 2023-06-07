using HalconDotNet;
using System.Windows;
using System.Windows.Controls;
using Point = VisionGuided.Point;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for HalconSmartControl.xaml
    /// </summary>
    public partial class HalconSmartControl : UserControl
    {
        public HalconSmartControl()
        {
            InitializeComponent();
        }

        public HObject Image
        {
            get { return (HObject)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public Point Point
        {
            get { return (Point)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Point.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(Point), typeof(HalconSmartControl), new PropertyMetadata(null, OnPointChanged));

        private static void OnPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HalconSmartControl halconSmartControl)
            {
                Point point = e.NewValue as Point;
                HWindow OutputHandle = halconSmartControl.HalconWindow.HalconWindow;
                HOperatorSet.SetColor(OutputHandle, "blue");
                HOperatorSet.DispCross(OutputHandle, point.X, point.Y, 20, 0);
            }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(HObject), typeof(HalconSmartControl), new PropertyMetadata(null, OnImageChanged));

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HalconSmartControl halconSmartControl)
            {
                HOperatorSet.DispImage(e.NewValue as HObject, halconSmartControl.HalconWindow.HalconWindow);
            }
        }
    }
}