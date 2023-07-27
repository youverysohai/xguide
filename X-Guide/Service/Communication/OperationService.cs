using TcpConnectionHandler.Server;
using VisionProvider.Interfaces;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.Service.Communication
{
    internal class OperationService
    {
        protected readonly IRepository<Calibration> _repository;
        protected readonly IVisionService _visionService;
        protected readonly IServerTcp _serverService;

        public OperationService(IRepository<Calibration> repository, IVisionService visionService, IServerTcp serverService)
        {
            _repository = repository;
            _visionService = visionService;
            _serverService = serverService;
        }
    }
}