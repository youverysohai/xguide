using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using System.Security;
using X_Guide.MessageToken;
using X_Guide.Validation;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.Service
{
    public class AuthenticationService
    {
        private readonly IRepository _repository;
        private readonly IMessenger _messenger;
        private User userModel;

        public User CurrentUser
        {
            get { return userModel; }
            set
            {
                userModel = value;
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public AuthenticationService(IRepository repository, IMessenger messenger)
        {
            _repository = repository;
            _messenger = messenger;
        }

        public bool Login(string username, SecureString password)
        {
            User user = Authenticate(username, password);
            if (user != null)
            {
                CurrentUser = user;
                _messenger.Send(new UserLoginChanged(user));
                return true;
            }
            return false;
        }

        private User Authenticate(string username, SecureString password)
        {
            User user = _repository.Find<User>(r => r.Username.Equals(username)).FirstOrDefault();
            return user;
        }

        public bool Register(User user, SecureString password)
        {
            user.PasswordHash = PasswordHashUtility.HashSecureString(password);
            _repository.Create(user);
            return true;
        }

        public async void Delete(int id)
        {
            await _repository.DeleteById<User>(id);
        }
    }
}