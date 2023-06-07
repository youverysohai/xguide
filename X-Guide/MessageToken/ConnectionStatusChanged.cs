using CommunityToolkit.Mvvm.Messaging.Messages;

namespace X_Guide.MessageToken
{
    internal class ConnectionStatusChanged : ValueChangedMessage<bool>
    {
        public ConnectionStatusChanged(bool value) : base(value)
        {
        }
    }
}