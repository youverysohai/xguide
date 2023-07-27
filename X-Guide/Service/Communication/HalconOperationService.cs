using System;
using System.Threading.Tasks;
using TcpConnectionHandler.Server;
using VisionProvider.Interfaces;

namespace X_Guide.Service.Communication
{
    internal class HalconOperationService : OperationService, IOperationService
    {
        public HalconOperationService(IVisionService visionService, IServerTcp serverService) : base(null, visionService, serverService)
        {
        }

        public Task<object> Operation(string[] parameter)
        {
            throw new NotImplementedException();
        }
    }
}