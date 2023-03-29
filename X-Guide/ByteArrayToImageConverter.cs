using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
   /* public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream((byte[])value))
            {
                memoryStream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }

           return bitmapImage; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            byte[] bytes = File.ReadAllBytes((string)parameter);
            return bytes;
        }*/
    
}
