using System;
using System.Collections.Generic;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.Service.Communation
{
    public class ServerCommand : IServerCommand, IDisposable
    {
        public Queue<string> commandQeueue = new Queue<string>();
        private readonly IServerService _serverService;
        private readonly IOperationService _operationService;

        public event EventHandler<object> OnOperationCalled;

        public ServerCommand(IServerService serverService, IClientService clientService, IVisionService visionService, ICalibrationDb calibDb, IOperationService operationService)
        {
            _serverService = serverService;
            _operationService = operationService;
            _serverService._dataReceived += ValidateSyntax;
        }

        public void ValidateSyntax(object sender, NetworkStreamEventArgs network)
        {
            string[] data = network.Data;

            switch (data[0].Trim().ToLower())
            {
                case "xguide": Operation(data); break;
                default: break;
            }
        }

        public void Operation(string[] parameter)
        {
            object operationData = _operationService.Operation(parameter);
            OnOperationCalled?.Invoke(this, operationData);
        }

        public void Dispose()
        {
            _serverService._dataReceived -= ValidateSyntax;
        }

        public void SubscribeOnOperationEvent(EventHandler<object> action)
        {
            OnOperationCalled += action;
        }

        public void UnsubscribeOnOperationEvent(EventHandler<object> action)
        {
            OnOperationCalled -= action;
        }
    }
}