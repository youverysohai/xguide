using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Security;

namespace X_Guide.MessageToken
{
    internal enum Request
    {
        Login,
        Register,
        Logout,
        Add,
        Delete,
    }
    internal class AuthenticationRequest : RequestMessage<bool>
    {
                  
        public readonly string Username;
        public readonly SecureString Password;
        public  Request Request { get; }

        public AuthenticationRequest(string username, SecureString password, Request request)
        {
            Username = username;
            Password = password;
            Request = request;
        }

    
     
    }
}
