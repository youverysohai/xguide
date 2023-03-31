using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IManipulatorDbService
    {

        
        void CreateManipulator(ManipulatorModel manipulator);
        Task<IEnumerable<string>> GetAllManipulatorName();
        Task<IEnumerable<ManipulatorModel>> GetAllManipulator();

        string GetManipulatorDelimiter(string name);
        Task<ManipulatorModel> GetManipulator(string name);
        Task<bool> SaveManipulator(ManipulatorModel manipulator);


        
    }
}
