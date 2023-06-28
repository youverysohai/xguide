using TcpConnectionHandler.Server;
using VisionProvider.Interfaces;
using XGuideSQLiteDB;

namespace X_Guide.Service.Communication
{
    internal class OperationService
    {
        protected readonly IRepository _repository;
        protected readonly IVisionService _visionService;
        protected readonly IServerTcp _serverService;

        public OperationService(IRepository repository, IVisionService visionService, IServerTcp serverService)
        {
            _repository = repository;
            _visionService = visionService;
            _serverService = serverService;
        }
    }
}