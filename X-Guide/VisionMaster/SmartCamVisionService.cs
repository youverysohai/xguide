using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;

namespace X_Guide.VisionMaster
{
    internal class SmartCamVisionService : IVisionService
    {
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

        public Task<Point> GetVisCenter()
        {
            throw new NotImplementedException();
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
