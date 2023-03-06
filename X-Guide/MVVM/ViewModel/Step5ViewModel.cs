using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase
    {
        private BitmapImage _videoSource;

        public BitmapImage VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }


        private FilterInfo _currentDevice;
        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set
            {
                _currentDevice = value;
                OnPropertyChanged();
            }
        }

        private ImageService _imageService;

        public ICommand VideoCommand { get; set; }



        public Step5ViewModel()
        {
            GetVideoDevices();
            _imageService = new ImageService(CurrentDevice);
            _imageService.videoFrameChanged += OnVideoFrameChanged;
            VideoCommand = new VideoCommand(_imageService);
      
        }

        private void OnVideoFrameChanged(object sender, BitMapImageArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => VideoSource = e.BitmapImage));
        }

        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevices.Add(filterInfo);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public override ViewModelBase GetNextViewModel()
        {
            return null;
        }
    }
}
