using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Point = VisionGuided.Point;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6TopConfigViewModel : ViewModelBase
    {
        public NinePointCalibrationViewModel NinePoint { get; }

        private readonly IMessenger _messenger;

        public RelayCommand StartVisionCalibration { get; private set; }
        private readonly ICalibrationService _calibrationService;
        public RelayCommand VisionCalibrationStartCommand { get; set; }

        public Step6TopConfigViewModel(ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger)
        {
            StartVisionCalibration = new RelayCommand(VisionCalib);
            NinePoint = ninePoint;
            _messenger = messenger;
            _calibrationService = calibrationService;
            VisionCalibrationStartCommand = new RelayCommand(StartVision9Point);
        }

        private async void VisionCalib()
        {
            Point[] points = await _messenger.Send(new NinePointData(DataType.Vision));
            foreach (var point in points)
            {
                Debug.WriteLine(point);
            }
        }

        private void StartVision9Point()
        {
            _calibrationService.LookingDownward9PointVision(BlockingCall);
        }

        private Task BlockingCall(int arg)
        {
            throw new NotImplementedException();
        }
    }
}