
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using X_Guide.CustomEventArgs;
using X_Guide.HelperClass;

namespace X_Guide.Service
{
    internal class ImageService
    {

  

     


        public event EventHandler<BitMapImageArgs> videoFrameChanged; 
        public ImageService()
        {

         
        }

        public void StartCamera()
        {
           
        }

        public void StopCamera()
        {
         
        }


        private void video_NewFrame(object sender)
        {
           
        }

        public void OnVideoFrameChanged(object sender, BitmapImage bi)
        {
            
            videoFrameChanged.Invoke(sender, new BitMapImageArgs(bi));
        }



   
}
}
