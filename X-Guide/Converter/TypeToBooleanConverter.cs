using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using X_Guide.Enums;

namespace X_Guide.Converter
{
    public class TypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is bool && (bool)value == false)
            //    return Visibility.Visible;
            //else
            //    return (parameter is Visibility) ? parameter : Visibility.Collapsed;
            switch (value) {
                case (int)ManipulatorType.SixAxis: return true;
                default: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
