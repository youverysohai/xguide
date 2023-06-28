using System;
using System.Threading.Tasks;
using TcpConnectionHandler.Server;
using VisionProvider.Interfaces;
using XGuideSQLiteDB;

namespace X_Guide.Service.Communication
{
    internal class HalconOperationService : OperationService, IOperationService
    {
        public HalconOperationService(IRepository repository, IVisionService visionService, IServerTcp serverService) : base(repository, visionService, serverService)
        {
        }

        public Task<object> Operation(string[] parameter)
        {
            throw new NotImplementedException();
        }
    }
}