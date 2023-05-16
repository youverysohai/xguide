using System.Collections.Generic;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IVisionDb
    {
        HikVisionModel Get();

        Task<bool> Delete(HikVisionModel vision);

        void Update(HikVisionModel vision);

        Task<IEnumerable<HikVisionModel>> GetAll();

        Task<bool> Add(HikVisionModel vision);
    }
}