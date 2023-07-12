using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6TopConfigViewModel : ViewModelBase
    {
        private readonly ICalibrationService _calibrationService;
        public RelayCommand VisionCalibrationStartCommand { get; set; }

        public Step6TopConfigViewModel(ICalibrationService calibrationService)
        {
            _calibrationService = calibrationService;
            VisionCalibrationStartCommand = new RelayCommand(StartVision9Point);
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