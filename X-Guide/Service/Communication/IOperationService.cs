using System.Threading.Tasks;

namespace X_Guide.Service.Communication
{
    public interface IOperationService
    {
        Task<object> Operation(string[] parameter);
    }
}