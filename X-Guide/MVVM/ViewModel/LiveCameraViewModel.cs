using X_Guide.MVVM.Command;

namespace X_Guide.MVVM.ViewModel
{
    internal class LiveCameraViewModel : ViewModelBase
    {
        public RelayCommand StartLiveImageCommand { get; set; }
        public RelayCommand StopLiveImageCommand { get; set; }

        private IVisionViewModel _visionView;
        private bool _canRun = true;

        public bool CanRun
        {
            get { return _canRun; }
            set
            {
                _canRun = value;
                StartLiveImageCommand.OnCanExecuteChanged();
                StopLiveImageCommand.OnCanExecuteChanged();
            }
        }

        public IVisionViewModel VisionView
        {
            get { return _visionView; }
            set { _visionView = value; }
        }

        public LiveCameraViewModel(IVisionViewModel visionView)
        {
            VisionView = visionView;
            StartLiveImageCommand = new RelayCommand(StartLiveImage, (obj) => _canRun);
            StopLiveImageCommand = new RelayCommand(StopLiveImage, (obj) => _canRun);
        }

        private void StopLiveImage(object obj)
        {
            CanRun = false;
            _visionView.StopLiveImage();
            CanRun = true;
        }

        private void StartLiveImage(object obj)
        {
            _visionView.StartLiveImage();
        }
    }
}