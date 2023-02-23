using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public void CreateUser(UserModel userModel)
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext()) { ;

            User user = new User
            {
      
                Email = userModel.Email,
                Username = userModel.Username,
                PasswordHash = "Password",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.SaveChanges();
            }
       
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
