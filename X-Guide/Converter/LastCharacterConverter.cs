using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Converter
{
    public class LastCharacterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CalibrationViewModel cm = value as CalibrationViewModel;
            if (string.IsNullOrEmpty(cm.Name))
            {
                return string.Empty;
            }
            return cm.Name.Substring(cm.Name.Length - 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
