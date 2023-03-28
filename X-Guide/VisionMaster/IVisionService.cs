using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.VisionMaster
{
    public interface IVisionService
    {
        Task<bool> ImportSol(string filepath);
    }
}
