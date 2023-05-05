using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;

namespace X_Guide.VisionMaster
{
    internal class HalcomVisionService : IVisionService
    {
        public void ConnectServer()
        {
            throw new NotImplementedException();
        }

        public List<VmProcedure> GetAllProcedures()
        {
            throw new NotImplementedException();
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

        public Task<Point> GetVisCenter()
        {
            throw new NotImplementedException();
        }

        public Task ImportSol(string filepath)
        {
            throw new NotImplementedException();
        }

        public void RunOnceAndSaveImage()
        {
            throw new NotImplementedException();
        }

        public Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            throw new NotImplementedException();
        }
    }
}