using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VM.Core;

namespace X_Guide.VisionMaster
{
    public class VisionService : IVisionService
    {
        public async Task<bool> ImportSol(string filepath)
        {

            //@"C:\Users\Xlent_XIR02\Desktop\livecam.sol"
            try
            {

                await Task.Run(() => VmSolution.Import(filepath));
                return true;
        /*        VmProcedure p = (VmProcedure)VmSolution.Instance["LiveCam"];
                p.Run();*/

                //p_box.LoadFrontendSource();

                //p_box.BindSingleProcedure(p.ToString());

                //p_box.AutoChangeSize();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
