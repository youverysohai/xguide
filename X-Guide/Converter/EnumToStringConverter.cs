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
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int enumValue)
            {
                return Enum.GetName(typeof(UserRole), value);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int enumValue)
            {
                return Enum.GetName(typeof(UserRole), value);
            }

            return string.Empty;
        }
    }
}
