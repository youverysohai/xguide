using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MessageToken;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.Service
{
    public class AuthenticationService : IRecipient<AuthenticationRequest>, IDisposable
    {
        private readonly IUserDb _userService;
        private readonly IMessenger _messenger;

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

        public AuthenticationService(IUserDb userService, IMessenger messenger)
        {
            _userService = userService;
            _messenger = messenger;
            
            _messenger.Register(this);
        }
        public async Task<bool> Login(string username, SecureString password)
        {
            UserModel user = await _userService.Authenticate(username, password);
             
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

   
        public async Task<bool> Register(UserModel user, SecureString password)
        {
            bool success = await _userService.Add(user, password);
            return success;
        }

        async void IRecipient<AuthenticationRequest>.Receive(AuthenticationRequest message)
        {
            switch (message.Request)
            {
                case Request.Login:  break;
                case Request.Logout: break;
            }
            bool abc = await Login(message.Username, message.Password);

            message.Reply(true);

            _messenger.Send(new UserLoginChanged(CurrentUser));
        }

        public void Dispose()
        {
            _messenger.UnregisterAll(this);
        }
    }
}
