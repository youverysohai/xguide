using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;

namespace X_Guide.VisionMaster
{
    public interface IVisionService
    {
        Task<VmProcedure> ImportSol(string filepath);

        List<string> GetAllProcedureName();
        void RunOnceAndSaveImage();
    }
}
