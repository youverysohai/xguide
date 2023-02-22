using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.DTOs;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.UserProviders
{
    internal class DatabaseUserProvider : IUserProvider
    {
        private readonly DbContextFactory _userDbContextFactory;

        public DatabaseUserProvider(DbContextFactory userDbContextFactory)
        {
            _userDbContextFactory = userDbContextFactory;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
            {
                IEnumerable<User> users = await context.Users.ToListAsync();
                return users.Select(r => new UserModel(r.Username, r.Email, r.PasswordHash.ToString()));
            }
        }


       
    }
}
