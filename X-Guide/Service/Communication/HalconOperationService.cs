using System;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.VisionMaster;
using XGuideSQLiteDB;

namespace X_Guide.Service.Communication
{
    internal class HalconOperationService : OperationService, IOperationService
    {
        public HalconOperationService(IRepository repository, IVisionService visionService, IServerService serverService) : base(repository, visionService, serverService)
        {
        }

        public Task<object> Operation(string[] parameter)
        {
            throw new NotImplementedException();
        }
    }
}