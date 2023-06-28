using System.Runtime.Versioning;
using TcpConnectionHandler;
using TcpConnectionHandler.Client;
using VisionProvider.Interfaces;
using VM.Core;
using VMControls.Interface;
using Point = VisionGuided.Point;

namespace HikVisionProvider
{
    [SupportedOSPlatform("windows")]
    public class HikVisionService : VisionService, IVisionService
    {
        private readonly IClientTcp _clientService;
        private readonly string _solutionPath;
        private readonly CancellationTokenSource? _cts;
        public string? Procedure { get; set; }

        public HikVisionService(string solutionPath, IClientTcp clientService)
        {
            _solutionPath = solutionPath;
            _clientService = clientService;
            ImportSol(_solutionPath);
        }

        /// <inheritdoc/>

        public async Task<Point?> GetVisCenter()
        {
            throw new NotImplementedException();
            //await _clientService.WriteDataAsync($"XGUIDE,{Procedure}");
            //_cts = new CancellationTokenSource();
            //var timer = new Timer(20000);
            //timer.Elapsed += (s, e) => _cts.Cancel();
            //timer.AutoReset = false;
            //timer.Start();
            //Point? point = await _clientService.RegisterSingleRequestHandler(GetVisCenterEvent, _cts.Token);
            //timer.Dispose();
            //Debug.WriteLine(point);
            //return point;
        }

        private Point? GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            throw new NotImplementedException();
            //string[]? data = e.Data;

            //if (data?.Length == 4)
            //{
            //    if (data[1] != "")
            //    {
            //        return new Point(double.Parse(data[1]), -double.Parse(data[2]), double.Parse(data[3]));
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //throw new Exception($"{this} : Data not found!");
        }

        public async Task<List<VmProcedure>> GetAllProcedures()
        {
            //List<VmProcedure> vmProcedure = new List<VmProcedure>();
            //VmSolution.Instance.GetAllProcedureObjects(ref vmProcedure);
            //return await Task.FromResult(vmProcedure);
            throw new NotImplementedException();
        }

        public VmProcedure? GetProcedure(string name)
        {
            throw new NotImplementedException();
            //return VmSolution.Instance[name] as VmProcedure;
        }

        public List<VmModule> GetModules(VmProcedure vmProcedure)
        {
            //if (vmProcedure is null)
            //{
            //    throw new ArgumentNullException("Procedure is null");
            //}
            //return vmProcedure.Modules.ToList();
            throw new NotImplementedException();
        }

        public void StopProcedure()
        {
            //VmSolution.Instance.ContinuousRunEnable = false;
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        ///

        public bool ImportSol(string filepath)
        {
            //try
            //{
            //    if (VmSolution.Instance.SolutionPath != filepath)
            //    {
            //        VmSolution.Load(filepath);
            //        _clientService.ConnectServer();
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show(ex.ToString());
            //    return false;
            //}
            throw new NotImplementedException();
        }

        /// <inheritdoc/>

        public IVmModule? RunProcedure(string name, bool continuous = false)
        {
            //if (!(VmSolution.Instance[name] is VmProcedure procedure)) return null;
            //if (procedure.ContinuousRunEnable) return procedure;

            //if (continuous) procedure.ContinuousRunEnable = true;
            //else procedure.Run();
            //return procedure;
            throw new NotImplementedException();
        }
    }
}