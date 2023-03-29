using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VM.Core;
using VMControls.Interface;

namespace X_Guide.VisionMaster
{
    public class VisionService : IVisionService
    {
        public List<string> GetAllProcedureName()
        {
             return  VmSolution.Instance.GetAllProcedureList().astProcessInfo.Where(x => x.strProcessName != null).ToList().Select(x=> x.strProcessName).ToList();  
        }

        public async Task<IVmModule> GetVmModule(string name)
        {
            return await Task.Run(() => VmSolution.Instance[$"{name}"] as IVmModule);
        }

        public async Task<VmProcedure> ImportSol(string filepath)
        {

            //@"C:\Users\Xlent_XIR02\Desktop\livecam.sol"
            try
            {

                return await Task.Run(() => { VmSolution.Import(filepath); return VmSolution.Instance["Live"] as VmProcedure; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

    

        public void RunOnceAndSaveImage()
        {
            throw new NotImplementedException();
        }
    }
}
