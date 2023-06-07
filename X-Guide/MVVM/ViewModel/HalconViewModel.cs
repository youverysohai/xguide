using CommunityToolkit.Mvvm.Messaging;
using HalconDotNet;
using System;
using VisionGuided;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class HalconViewModel : ViewModelBase, IVisionViewModel, IRecipient<HObject>
    {
        public HObject Image { get; set; }

        private readonly HalconVisionService _visionService;
        private readonly IMessenger _messenger;

        public HObject OutputImage { get; set; }
        public Point OutputImageParameter { get; set; }
        public HObject LiveImage { get; set; }

        public RelayCommand LiveImageCommand { get; set; }

        public HalconViewModel(IVisionService visionService, IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<HObject>(this);
            _visionService = (HalconVisionService)visionService;
        }

        public override void Dispose()
        {
            _messenger.Unregister<HObject>(this);
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

        public void Receive(HObject message)
        {
            Image = message;
        }
    }
}