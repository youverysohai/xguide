using AForge.Video;
using AForge.Video.DirectShow;
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

        private IVideoSource _videoSource;

     
        public FilterInfo CurrentDevice { get; set; }

        public event EventHandler<BitMapImageArgs> videoFrameChanged; 
        public ImageService(FilterInfo currentDevice)
        {

            CurrentDevice = currentDevice;

        }

        public void StartCamera()
        {
            if (CurrentDevice != null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
            }
        }

        public void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                OnVideoFrameChanged(this, bi);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        public void OnVideoFrameChanged(object sender, BitmapImage bi)
        {
            
            videoFrameChanged.Invoke(sender, new BitMapImageArgs(bi));
        }



   
}
}
