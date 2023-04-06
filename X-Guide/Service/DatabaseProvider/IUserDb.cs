using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IUserDb
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<bool> Add(UserModel user, SecureString password);
        Task<UserModel>Authenticate(string username, SecureString password);
    }
}
