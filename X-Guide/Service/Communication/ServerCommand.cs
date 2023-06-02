using System;
using System.Collections.Generic;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;

namespace X_Guide.Service.Communation
{
    public class ServerCommand : IServerCommand, IDisposable
    {
        public Queue<string> commandQeueue = new Queue<string>();
        private readonly IEventAggregator _eventAggregator;
        private readonly IServerService _serverService;
        private readonly IOperationService _operationService;

        public ServerCommand(IServerService serverService, IOperationService operationService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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

        public async void Operation(string[] parameter)
        {
            object operationData = await _operationService.Operation(parameter);
           }

        public void Dispose()
        {
            _serverService._dataReceived -= ValidateSyntax;
        }
    }
}