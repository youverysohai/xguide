using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VisionGuided;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;

namespace X_Guide.VisionMaster
{
    internal class SmartCamVisionService : IVisionService
    {
        private readonly IClientService _clientService;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public SmartCamVisionService(IClientService clientService)
        {
            _clientService = clientService;
            _clientService.ConnectServer();
        }

        public VmModule GetCameras()
        {
            throw new NotImplementedException();
        }

        public List<VmModule> GetModules(VmProcedure vmProcedure)
        {
            return null;
        }

        public VmProcedure GetProcedure(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Point> GetVisCenter()
        {
            await _clientService.WriteDataAsync($"Capture");

            Point point = await _clientService.RegisterSingleRequestHandler(GetVisCenterEvent, _cts.Token);
            Debug.WriteLine(point);
            return point;
        }

        private Point GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            string[] data = e.Data;

            if (data.Length == 5)
            {
                if (data[1] != "")
                {
                    return new Point(double.Parse(data[1]), -double.Parse(data[2]), -double.Parse(data[3]));
                }
                else
                {
                    return null;
                }
            }
            throw new Exception($"{this} : Data not found!");
        }

        public Task ImportSolAsync(string filepath)
        {
            throw new NotImplementedException();
        }

        public Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<VmProcedure>> GetAllProcedures()
        {
            return Task.FromResult(new List<VmProcedure>());
        }
    }
}