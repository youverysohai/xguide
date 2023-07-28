using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Tools.Extension;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using X_Guide.Extension.Model;
using X_Guide.MVVM.ViewModel;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using Point = VisionGuided.Point;

namespace X_Guide.CustomControls.Layouts
{
    /// <summary>
    /// Interaction logic for NineBoxCalibLayout.xaml
    /// </summary>
    /// 

 
    public class IndexToGridConverter : MarkupExtension, IValueConverter
    {
  
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int index && parameter is string type)
            {
                return type.ToLower() == "row" ? index / 3 : index % 3;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public partial class NineBoxCalibLayout : UserControl
    {

        public ObservableCollection<Point> NinePoint
        {
            get { return (ObservableCollection<Point>)GetValue(NinePointProperty); }
            set { SetValue(NinePointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NinePoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NinePointProperty =
            DependencyProperty.Register("NinePoint", typeof(ObservableCollection<Point>), typeof(NineBoxCalibLayout), new PropertyMetadata(null));



        public int CurrentPosition
        {
            get { return (int)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register("CurrentPosition", typeof(int), typeof(NineBoxCalibLayout), new PropertyMetadata(0));


        public NineBoxCalibLayout()
        {
            InitializeComponent();
        }
    }
}
