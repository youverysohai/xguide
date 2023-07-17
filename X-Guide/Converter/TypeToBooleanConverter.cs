using System;
using System.Globalization;
using System.Windows.Data;
using XGuideSQLiteDB.Models;

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
            switch (value)
            {
                case (int)ManipulatorType.SixAxis: return true;
                case (int)ManipulatorType.GantrySystemWR: return true;
                default: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseTypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case (int)ManipulatorType.SixAxis: return true;
                case (int)ManipulatorType.GantrySystemWR: return false;
                default: return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}