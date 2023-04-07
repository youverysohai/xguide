
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    internal class JogRobotViewModel:ViewModelBase
    {
        private BitmapImage _videoSource;

        public BitmapImage VideoSource
        {
            get { return _videoSource; }
            set
            {
                _videoSource = value;
                OnPropertyChanged();
            }
        }

 

        private ImageService _imageService;

        public ICommand VideoCommand { get; set; }



        public JogRobotViewModel()
        {
            GetVideoDevices();
       

        }

        private void OnVideoFrameChanged(object sender, BitMapImageArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => VideoSource = e.BitmapImage));
        }

        private void GetVideoDevices()
        {
     
        }
    }
}
