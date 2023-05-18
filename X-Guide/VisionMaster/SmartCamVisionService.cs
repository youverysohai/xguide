using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using Timer = X_Guide.HelperClass.Timer;

namespace X_Guide.VisionMaster
{
    internal class SmartCamVisionService : IVisionService
    {
        private readonly IClientService _clientService;
        private CancellationTokenSource _cts = new CancellationTokenSource();

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
            throw new NotImplementedException();
        }

        public VmProcedure GetProcedure(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Point> GetVisCenter()
        {
            await _clientService.WriteDataAsync($"XGUIDE,Give_Me_Vision_Center(x,y,theta)");
    
            Point point = await _clientService.RegisterSingleRequestHandler(GetVisCenterEvent, _cts.Token);
            Debug.WriteLine(point);
            return point;
        }

        private Point GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            string[] data = e.Data;

            if (data.Length == 4)
            {
                if (data[1] != "")
                {
                    return new Point(double.Parse(data[1]), -double.Parse(data[2]), double.Parse(data[3]));
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
    }
}
