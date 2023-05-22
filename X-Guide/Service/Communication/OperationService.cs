using X_Guide.Communication.Service;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.Service.Communication
{
    internal class OperationService
    {
        protected readonly ICalibrationDb _calibrationDb;
        protected readonly IVisionService _visionService;
        protected readonly IServerService _serverService;

        public OperationService(ICalibrationDb calibrationDb, IVisionService visionService, IServerService serverService)
        {
            _calibrationDb = calibrationDb;
            _visionService = visionService;
            _serverService = serverService;
        }
    }
}