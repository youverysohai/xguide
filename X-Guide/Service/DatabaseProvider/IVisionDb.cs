using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IVisionDb
    {
        Task<VisionModel> Get(string name);

        Task<bool> Delete(string name);
        Task<bool> Update(VisionModel vision);

        Task<IEnumerable<VisionModel>> GetAll();




    }
}
