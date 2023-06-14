using ModernWpf.Controls;
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
using X_Guide.Enums;
using X_Guide.MVVM.ViewModel;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for MotionCalibrationControl.xaml
    /// </summary>
    public partial class MotionCalibrationControl : UserControl
    {


        public int Speed
        {
            get { return (int)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Speed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(int), typeof(MotionCalibrationControl), new PropertyMetadata(0));



        public int Acceleration
        {
            get { return (int)GetValue(AccelerationProperty); }
            set { SetValue(AccelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Acceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccelerationProperty =
            DependencyProperty.Register("Acceleration", typeof(int), typeof(MotionCalibrationControl), new PropertyMetadata(0));



        public int MotionDelay
        {
            get { return (int)GetValue(MotionDelayProperty); }
            set { SetValue(MotionDelayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MotionDelay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MotionDelayProperty =
            DependencyProperty.Register("MotionDelay", typeof(int), typeof(MotionCalibrationControl), new PropertyMetadata(0));



        public int XOffset
        {
            get { return (int)GetValue(XOffsetProperty); }
            set { SetValue(XOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XOffsetProperty =
            DependencyProperty.Register("XOffset", typeof(int), typeof(MotionCalibrationControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int YOffset
        {
            get { return (int)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YOffsetProperty =
            DependencyProperty.Register("YOffset", typeof(int), typeof(MotionCalibrationControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public int JointRotationAngle
        {
            get { return (int)GetValue(JointRotationAngleProperty); }
            set { SetValue(JointRotationAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JointRotationAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JointRotationAngleProperty =
            DependencyProperty.Register("JointRotationAngle", typeof(int), typeof(MotionCalibrationControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public ManipulatorType ManipulatorType
        {
            get { return (ManipulatorType)GetValue(ManipulatorTypeProperty); }
            set { SetValue(ManipulatorTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ManipulatorType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ManipulatorTypeProperty =
            DependencyProperty.Register("ManipulatorType", typeof(ManipulatorType), typeof(MotionCalibrationControl), new FrameworkPropertyMetadata(ManipulatorType.GantrySystemWR, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        



        public bool IsManual
        {
            get { return (bool)GetValue(IsManualProperty); }
            set { SetValue(IsManualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsManual.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsManualProperty =
            DependencyProperty.Register("IsManual", typeof(bool), typeof(MotionCalibrationControl), new PropertyMetadata(false, OnEnabledChanged));





        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MotionCalibrationControl motionCalibration)
            {
                bool isManual = (bool)e.NewValue;
                motionCalibration.Slider1.IsEnabled = !isManual;
                motionCalibration.Slider2.IsEnabled = !isManual;
                motionCalibration.Slider3.IsEnabled = !isManual;
                motionCalibration.NumberBoxSpeed.IsEnabled = !isManual;
                motionCalibration.NumberBoxAccel.IsEnabled = !isManual;
                motionCalibration.NumberBoxMotionDelay.IsEnabled = !isManual;
            }
        }

        public MotionCalibrationControl()
        {

            InitializeComponent();
        }

        private void Slider_SpeedValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SpeedAngle != null)
            {
                SpeedAngle.Angle = (int)(e.NewValue * 1.7 - 85);
                SpeedValue.Text = ((int)e.NewValue).ToString();
            }
        }
        private void Slider_AccelerationValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AccelerationAngle != null)
            {
                AccelerationAngle.Angle = (int)(e.NewValue * 1.7 - 85);
                AccelValue.Text = ((int)e.NewValue).ToString();
            }
        }
    }
}
