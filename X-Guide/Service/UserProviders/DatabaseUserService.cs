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
    internal class DatabaseUserService : IUserService
    {
        private readonly DbContextFactory _userDbContextFactory;

        public DatabaseUserService(DbContextFactory userDbContextFactory)
        {
            _userDbContextFactory = userDbContextFactory;
        }


        public bool CheckPassword(string password)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(UserModel userModel)
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
            {
                User user = new User
                {
                    Email = userModel.Email,
                    Username = userModel.Username,
                    PasswordHash = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
            return true;
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
