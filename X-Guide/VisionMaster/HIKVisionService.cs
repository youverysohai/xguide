using HalconDotNet;
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
    public class HikVisionService : IVisionService
    {
        private readonly IClientService _clientService;
        private CancellationTokenSource _cts;

        public string Procedure { get; set; }

        public HikVisionService(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <inheritdoc/>

        public async Task<Point> GetVisCenter()
        {
            await _clientService.WriteDataAsync($"XGUIDE,{Procedure}");
            _cts = new CancellationTokenSource();
            var timer = new Timer(20000, (s, o) => _cts.Cancel());
            timer.Start();
            Point point = await _clientService.RegisterSingleRequestHandler(GetVisCenterEvent, _cts.Token);
            timer.Dispose();
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

        public List<VmProcedure> GetAllProcedures()
        {
            List<VmProcedure> vmProcedure = new List<VmProcedure>();
            VmSolution.Instance.GetAllProcedureObjects(ref vmProcedure);
            return vmProcedure;
        }

        public VmProcedure GetProcedure(string name)
        {
            return VmSolution.Instance[name] as VmProcedure;
        }

        public List<VmModule> GetModules(VmProcedure vmProcedure)
        {
            if (vmProcedure is null)
            {
                throw new ArgumentNullException("Procedure is null");
            }
            return vmProcedure.Modules.ToList();
        }

        public VmModule GetCameras()
        {
            return VmSolution.Instance["Basler1"] as VmModule;
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
                   if (VmSolution.Instance.SolutionPath != filepath)
                   {
                       VmSolution.Load(filepath);
                       _clientService.ConnectServer();
                   }
               }
               catch
               {
                   throw new CriticalErrorException(StrRetriver.Get("C000"));
               }
           });
        }

        /// <inheritdoc/>

        public async Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            if (!(VmSolution.Instance[name] is VmProcedure procedure)) return null;
            if (procedure.ContinuousRunEnable) return procedure;

            if (continuous) procedure.ContinuousRunEnable = true;
            else procedure.Run();
            return procedure;
        }

        public HObject GetImage()
        {
            throw new NotImplementedException();
        }
    }
}