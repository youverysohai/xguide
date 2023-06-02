using HalconDotNet;
using System;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class HalconViewModel : ViewModelBase, IVisionViewModel
    {
        public HObject Image { get; set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly HalconVisionService _visionService;
        public HObject OutputImage { get; set; }
        public Point OutputImageParameter { get; set; }
        public HObject LiveImage { get; set; }

        public RelayCommand LiveImageCommand { get; set; }

        public HalconViewModel(IVisionService visionService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<HObject>(OnImageReturn);
            _visionService = (HalconVisionService)visionService;
        }

        private void OnImageReturn(HObject image)
        {
            Image = image;
        }

        public override void Dispose()
        {
            _eventAggregator.Unsubscribe<HObject>(OnImageReturn);
            base.Dispose();
        }

        public void SetConfig(CalibrationViewModel calibrationViewModel)
        {
            throw new NotImplementedException();
        }

        public void StopLiveImage()
        {
            _visionService.StopGrabbingImage();
        }

        public void StartLiveImage()
        {
            _visionService.StartGrabbingImage();
        }

        public void ShowOutputImage()
        {
            throw new NotImplementedException();
        }
    }
}