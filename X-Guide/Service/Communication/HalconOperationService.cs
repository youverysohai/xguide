using System;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.Service.Communication
{
    internal class HalconOperationService : OperationService, IOperationService
    {
        public HalconOperationService(ICalibrationDb calibrationDb, IVisionService visionService, IServerService serverService) : base(calibrationDb, visionService, serverService)
        {
        }

        public Task<object> Operation(string[] parameter)
        {
            throw new NotImplementedException();
        }
    }
}