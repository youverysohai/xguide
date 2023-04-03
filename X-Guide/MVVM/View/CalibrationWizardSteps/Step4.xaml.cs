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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Globalization.NumberFormatting;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step4.xaml
    /// </summary>
    public partial class Step4 : UserControl
    {
        public Step4()
        {
            InitializeComponent();
        }
        private void SetNumberBoxNumberFormatter()
        {
           
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
