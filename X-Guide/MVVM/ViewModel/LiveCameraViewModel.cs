namespace X_Guide.MVVM.ViewModel
{
    internal class LiveCameraViewModel : ViewModelBase
    {
        private IVisionViewModel _visionView;

        public IVisionViewModel VisionView
        {
            get { return _visionView; }
            set { _visionView = value; }
        }

        public LiveCameraViewModel(IVisionViewModel visionView)
        {
            VisionView = visionView;
            visionView.StartLiveImage();
        }
    }
}