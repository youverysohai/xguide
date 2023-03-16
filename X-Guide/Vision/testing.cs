using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;

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

                p = (VmProcedure)VmSolution.Instance["Flow1"];

                p.Run();


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
