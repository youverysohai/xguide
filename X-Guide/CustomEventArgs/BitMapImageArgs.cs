using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace X_Guide.CustomEventArgs
{
    public class BitMapImageArgs
    {
        public BitmapImage BitmapImage { get; set; }

        public BitMapImageArgs(BitmapImage bitmapImage)
        {
            BitmapImage = bitmapImage;
        }
    }
}
