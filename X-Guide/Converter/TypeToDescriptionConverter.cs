using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using X_Guide.Enums;
using XGuideSQLiteDB.Models;

namespace X_Guide.Converter
{
    public class TypeToDescriptionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            int integer = (int)value;
            return EnumHelperClass.GetEnumDescription<ManipulatorType>(integer);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}