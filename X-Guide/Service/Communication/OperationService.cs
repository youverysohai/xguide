using X_Guide.Communication.Service;
using X_Guide.VisionMaster;
using XGuideSQLiteDB;

namespace X_Guide.Service.Communication
{
    internal class OperationService
    {
        protected readonly IRepository _repository;
        protected readonly IVisionService _visionService;
        protected readonly IServerService _serverService;

        public OperationService(IRepository repository, IVisionService visionService, IServerService serverService)
        {
            _repository = repository;
            _visionService = visionService;
            _serverService = serverService;
        }
    }
}