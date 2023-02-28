using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal interface IMachineService
    {

        
        void CreateMachine(MachineModel machine);
        IEnumerable<string> GetAllMachineName();
        IEnumerable<MachineModel> GetAllMachine();
        MachineModel GetMachine(string name);
        void SaveMachine(MachineModel machine);
        
    }
}
