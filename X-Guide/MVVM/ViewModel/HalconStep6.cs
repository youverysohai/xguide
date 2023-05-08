using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Service;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    class HalconStep6 :ViewModelBase
    {
        private readonly IVisionService _visionService;
        private readonly ICalibrationService _calibrationService;
        public RelayCommand CalibrationCommand { get; set; }
        public HalconStep6(IVisionService visionService, ICalibrationService calibrationService) {
            _visionService = visionService;
            _calibrationService = calibrationService;
             CalibrationCommand = new RelayCommand(Calibrate);

        }

        private void Calibrate(object obj)
        {

        }
    }
}
