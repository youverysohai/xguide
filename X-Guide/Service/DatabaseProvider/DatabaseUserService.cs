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
using X_Guide.Validation;

namespace X_Guide.Service.DatabaseProvider
{
    internal class DatabaseUserService : IUserService
    {
        private readonly DbContextFactory _userDbContextFactory;

        public DatabaseUserService(DbContextFactory userDbContextFactory)
        {
            _userDbContextFactory = userDbContextFactory;
        }


     /*   This method checks if the provided password matches the hashed password stored in the database for the specified user.*/
        public bool CheckPassword(string username, string password)
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
            {
                var user = context.Users.SingleOrDefault(r => r.Username == username);
                if (user != null)
                {
                    var hashedPassword = PasswordHashUtility.HashSecureString(password);
                    if (hashedPassword == user.PasswordHash)
                    {
                        return true;
                    }
                }

                return false;

            }
        }

        public void CreateUser(UserModel userModel)
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
            {

                User user = new User
                {

                    Email = userModel.Email,
                    Username = userModel.Username,
                    PasswordHash = PasswordHashUtility.HashPassword(userModel.PasswordHash),
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
