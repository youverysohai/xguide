using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IMachineDbService
    {

        
        void CreateMachine(MachineModel machine);
        Task<IEnumerable<string>> GetAllMachineName();
        Task<IEnumerable<MachineModel>> GetAllMachine();

        string GetMachineDelimiter(string name);
        Task<MachineModel> GetMachine(string name);
        Task<bool> SaveMachine(MachineModel machine);


        
    }
}
