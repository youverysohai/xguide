using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.Service
{
    public class AuthenticationService
    {
        private IUserDbService _userService;
        public event Action CurrentUserChanged;

        private UserModel userModel;

        public UserModel CurrentUser
        {
            get { return userModel; }
            set { userModel = value;
                OnCurrentUserChanged();
            }
        }

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }
        public bool IsLoggedIn => CurrentUser != null;

        public AuthenticationService(IUserDbService userService)
        {
            _userService = userService;
        }
        public async Task<bool> Login(string username, SecureString password)
        {
            UserModel user = await _userService.AuthenticateUser(username, password);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public async Task<bool> Register(UserModel user, SecureString password)
        {
            bool success = await _userService.CreateUser(user, password);
            return success;
        }
    }
}
