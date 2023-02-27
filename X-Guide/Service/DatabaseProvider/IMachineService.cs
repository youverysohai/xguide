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

        Task<IEnumerable<MachineModel>> GetAllUsersAsync();
        void CreateMachine(MachineModel machine);

        IEnumerable<MachineModel> GetAllMachine();
        MachineModel GetMachine(string name);
        void SaveMachine(MachineModel machine);
        
    }
}
