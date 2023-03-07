using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.Service
{
    public class AuthenticationService
    {

        private IUserService _userService;

        
        public User CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;

        public AuthenticationService(IUserService userService)
        {
           _userService = userService;
        }
        public async Task<bool> Login(string username, SecureString password)
        {
            User user = await _userService.AuthenticateUser(username, password);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public async Task<bool> Register()
        {
            throw new NotImplementedException();
        }
    }
}
