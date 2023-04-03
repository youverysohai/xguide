using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using Xlent_Vision_Guided;

namespace X_Guide.VisionMaster
{
    public class VisionService : IVisionService
    {
        private readonly IClientService _clientService;
        public string Procedure { get; set; }
        public VisionService(IClientService clientService)
        {
            _clientService = clientService;
            
        }


        public async Task ConnectServer()
        {
            await _clientService.ConnectServer();
        }

        public async Task<Point> GetVisCenter()
        {
            await _clientService.WriteDataAsync($"XGUIDE,{Procedure}");
            Point point = await Task.Run(() => _clientService.RegisterRequestEventHandler(GetVisCenterEvent));
            Debug.WriteLine(point);
            if(point is null)
            {
                throw new Exception("Center point is null!");
            }
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

        public List<string> GetAllProcedureName()
        {
             return  VmSolution.Instance.GetAllProcedureList().astProcessInfo.Where(x => x.strProcessName != null).ToList().Select(x=> x.strProcessName).ToList();  
        }

        public async Task<IVmModule> GetVmModule(string name)
        {
            return await Task.Run(() => VmSolution.Instance[$"{name}"] as IVmModule);
        }

        public async Task<bool> ImportSol(string filepath)
        {
            try
            {

                await Task.Run(() =>  VmSolution.Import(filepath));
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }


        public void RunOnceAndSaveImage()
        {
            throw new NotImplementedException();
        }

        public void RunProcedure(string name, bool continuous = false)
        {
        
            VmProcedure procedure = VmSolution.Instance[$"{name}"] as VmProcedure;
            if (continuous) procedure.ContinuousRunEnable = true;
            else
            {
                procedure.Run();
            }
            Procedure = name;   
        }

    }
}
