using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows;

namespace X_Guide.MessageToken
{
    internal enum BoxState
    {
        Normal,
        Warning,
        Error,
    }

    internal class MessageBoxRequest : RequestMessage<MessageBoxResult>
    {
        public readonly string Message;
        public readonly BoxState State;

        public MessageBoxRequest(string message, BoxState state)
        {
            Message = message;
            State = state;
        }
    }
}