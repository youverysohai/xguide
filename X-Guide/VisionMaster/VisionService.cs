using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using Timer = X_Guide.HelperClass.Timer;

namespace X_Guide.VisionMaster
{
    public class VisionService : IVisionService
    {
        private readonly IClientService _clientService;
        private CancellationTokenSource _cts;

        public string Procedure { get; set; }

        public VisionService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async void ConnectServer()
        {
            await _clientService.ConnectServer();
        }

        /// <inheritdoc/>

        public async Task<Point> GetVisCenter()
        {
            await _clientService.WriteDataAsync($"XGUIDE,{Procedure}");
            _cts = new CancellationTokenSource();
            var timer = new Timer(5000, (s, o) => _cts.Cancel());
            timer.Start();
            Point point = await _clientService.RegisterSingleRequestHandler(GetVisCenterEvent, _cts.Token) ?? throw new Exception(StrRetriver.Get("VI000"));
            timer.Dispose();
            Debug.WriteLine(point);
            return point;
        }

        private Point GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            string[] data = e.Data;

            if (data.Length == 4)
            {
                if (data[0] == "1")
                {
                    return new Point(double.Parse(data[1]), -double.Parse(data[2]), double.Parse(data[3]));
                }
                else
                {
                    return null;
                }
            }
            throw new Exception("Data not found!");
        }

        public IEnumerable<string> GetProcedureNames()
        {
            return VmSolution.Instance.GetAllProcedureList().astProcessInfo.Where(x => x.strProcessName != null).ToList().Select(x => x.strProcessName).ToList();
        }

        public void GetCameras()
        {
            var i = VmSolution.Instance;
        }

        public async Task<IVmModule> GetVmModule(string name)
        {
            return await Task.Run(() => VmSolution.Instance[$"{name}"] as IVmModule);
        }

        /// <inheritdoc/>

        public async Task ImportSol(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new Exception(StrRetriver.Get("VI002"));
            }
            if (!Path.GetExtension(filepath).Equals(".sol", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(StrRetriver.Get("VI003"));
            }
            await Task.Run(() =>
           {
               try
               {
                   VmSolution.Load(filepath);
               }
               catch (Exception ex)
               {
                   throw new CriticalErrorException(StrRetriver.Get("C000"));
               }
           });
        }

        public void RunOnceAndSaveImage()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>

        public async Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            return await Task.Run(() =>
            {
                if (!(VmSolution.Instance[$"{name}"] is VmProcedure procedure))
                {
                    return null;
                }

                if (continuous) procedure.ContinuousRunEnable = true;
                else
                {
                    procedure.Run();
                }

                Procedure = name;
                return procedure;
            });
        }
    }
}