using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IManipulatorDb
    {


        Task<bool> Add(ManipulatorModel manipulator);
        Task<IEnumerable<string>> GetAllNames();
        Task<IEnumerable<ManipulatorModel>> GetAll();

        Task<bool> Delete(ManipulatorModel manipulator);
        string GetDelimiter(string name);
        Task<ManipulatorModel> Get(string name);
        Task<ManipulatorModel> Get(int id);
        Task<bool> Update(ManipulatorModel manipulator);


        
    }
}
