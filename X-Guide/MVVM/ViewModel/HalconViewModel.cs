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
        private readonly HalconVisionService _visionService;

        public event EventHandler<HObject> OnImageRecieved;

        public HObject OutputImage { get; set; }
        public Point OutputImageParameter { get; set; }
        public HObject LiveImage { get; set; }

        public RelayCommand LiveImageCommand { get; set; }

        public HalconViewModel(IVisionService visionService)
        {
            _visionService = (HalconVisionService)visionService;
            _visionService.OnImageReturn += HalcomViewModel_OnImageReturn;
        }

        private void HalcomViewModel_OnImageReturn(object sender, HObject e)
        {
            Image = e;
        }

        public override void Dispose()
        {
            _visionService.OnImageReturn -= HalcomViewModel_OnImageReturn;
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