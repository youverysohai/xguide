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
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communation;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step5ViewModel : ViewModelBase
    {
        private BitmapImage _videoSource;

        private int _jogDistance;

        public int JogDistance
        {
            get { return _jogDistance; }
            set { _jogDistance = value; }
        }

        public BitmapImage VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        private CancellationToken cancelJog = new CancellationToken();


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

        public ICommand ReportCommand { get; }
        public ICommand JogCommand { get; }

        private readonly CalibrationViewModel _setting;
        private readonly ServerCommand _serverCommand;

        public ICommand VideoCommand { get; set; }

        public Step5ViewModel(CalibrationViewModel setting, ServerCommand serverCommand)
        {
            GetVideoDevices();
            _imageService = new ImageService(CurrentDevice);
            ReportCommand = new RelayCommand(Testing);
 
            JogCommand = new RelayCommand(Jog);
            _imageService.videoFrameChanged += OnVideoFrameChanged;
            VideoCommand = new VideoCommand(_imageService);
            _setting = setting;
            _serverCommand = serverCommand;
            StartJogging();
        }
  
        private void Jog(object parameter)
        {
            int x = 0, y = 0, z = 0, rx = 0, ry = 0, rz = 0;
            switch (parameter)
            {
                case "Y+": y = JogDistance;  break;
                case "Y-": y = -JogDistance; break;
                case "X+": x = JogDistance; break;
                case "X-": x = -JogDistance; break;
                case "Z+": x = JogDistance; break;
                case "Z-": x = -JogDistance; break;
                case "RZ+": rz = JogDistance; break;
                case "RZ-": rz = -JogDistance; break;
                default: break;


            }
            string command = String.Format("JOG,TOOL,{0},{1},{2},{3},{4},{5},{6},{7}\n", x, y, z, rx, ry, rz, _setting.Speed, _setting.Acceleration);
            _serverCommand.commandQeueue.Enqueue(command);
        }

        private void StartJogging()
        {
            var i = _serverCommand.getConnectedClient().First();
            if (i.Value == null) MessageBox.Show("No connected client found!");
            else
            {
                _serverCommand.StartJogCommand(cancelJog, i.Value);
            }
        }

        private void Testing(object obj)
        {
            MessageBox.Show(_setting.ToString());
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
            return new Step6ViewModel();
        }
    }
}
