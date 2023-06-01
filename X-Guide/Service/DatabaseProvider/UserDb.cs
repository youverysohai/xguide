﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.MVVM.DBContext;
using X_Guide.MVVM.DTOs;
using X_Guide.MVVM.Model;
using X_Guide.Validation;

namespace X_Guide.Service.DatabaseProvider
{
    internal class UserDb : DbServiceBase, IUserDb
    {
        private readonly DbContextFactory _userDbContextFactory;

        public UserDb(IMapper mapper,DbContextFactory userDbContextFactory) : base(userDbContextFactory,mapper)
        {
            _userDbContextFactory = userDbContextFactory;
        }


        /*   This method checks if the provided password matches the hashed password stored in the database for the specified user.*/
        public async Task<UserModel> Authenticate(string username, SecureString password)
        {

            return await Task.Run(() =>
            {
                using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
                {
                    var user = context.Users.SingleOrDefault(r => r.Username == username);
                    if (user != null)
                    {
                        var hashedPassword = PasswordHashUtility.HashSecureString(password);
                        if (hashedPassword == user.PasswordHash)
                        {
                            return DBToModel(user);
                        }
                    }

                    return null;

                }
            });

        }


        public async Task<bool> Add(UserModel userModel, SecureString password)
        {
            
            return await Task.Run(() =>
            {
                using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
                {

                    User user = new User
                    {

                        Email = userModel.Email,
                        Username = userModel.Username,
                        PasswordHash = PasswordHashUtility.HashSecureString(password),
                        Role = 0,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    try
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            });
         

        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            using (XGuideDBEntities context = _userDbContextFactory.CreateDbContext())
            {
                IEnumerable<User> users = await context.Users.ToListAsync();
                return context.Users.ToList().Select(r => MapTo<UserModel>(r));
            }
        }


        private UserModel DBToModel(User user)
        {
            return new UserModel
            {
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                UpdatedAt = user.UpdatedAt,
                Role = (int)user.Role,
            };
        }

        public async Task<bool> Update(UserModel user)
        {
            return await AsyncQuery((context) =>
            {

                var result = context.Users.Find(user.Id);

                if (result != null)
                {
                    _mapper.Map(user, result);
                    context.SaveChanges();
                    return true;
                }
                return false;
            });
        }
    }
}
