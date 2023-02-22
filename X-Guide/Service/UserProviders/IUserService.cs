using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.UserProviders
{
    internal interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        void CreateUser(UserModel user);
        bool CheckPassword(string password);
    }
}
