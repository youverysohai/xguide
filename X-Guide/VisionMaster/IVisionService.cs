using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using Xlent_Vision_Guided;

namespace X_Guide.VisionMaster
{
    public interface IVisionService
    {
        Task ConnectServer();
        Task<Point> GetVisCenter();
        Task<bool> ImportSol(string filepath);

        void RunProcedure(string name, bool continuous = false);
        List<string> GetAllProcedureName();
        Task<IVmModule> GetVmModule(string name); 
        void RunOnceAndSaveImage();
    }
}
