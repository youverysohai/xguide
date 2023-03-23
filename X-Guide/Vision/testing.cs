using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VMControls.WPF.Release;

namespace X_Guide.Vision
{
    public class testing
    {
        public void ImportSolution()
        {
            VmProcedure p;
        
            

            try
            {
                VmSolution.Import(@"C:\Users\Xlent_XIR02\Desktop\livecam.sol");

                VmSolution.CreatSolInstance();
                p = (VmProcedure)VmSolution.Instance["Flow1"];

                p.Run();

                //p_box.LoadFrontendSource();
                ////p_box.BindSingleProcedure(p.ToString());
                //p_box.AutoChangeSize();

            }
            catch
            {
                Debug.WriteLine("Everything is fine");
            }
            finally
            {
                Debug.WriteLine("Chun fault nia ma");
            }
        }
    }
}
