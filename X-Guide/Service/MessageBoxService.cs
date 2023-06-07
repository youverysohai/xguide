using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using X_Guide.MessageToken;

namespace X_Guide.Service
{
    internal class MessageBoxService : IMessageBoxService, IRecipient<MessageBoxRequest>
    {
        public MessageBoxService(IMessenger messenger)
        {
            messenger.Register(this);
        }

        public void Receive(MessageBoxRequest message)
        {
            MessageBoxResult result;
            switch (message.State)
            {
                case BoxState.Normal: result = ShowMessage(message.Message); break;
                case BoxState.Warning: result = ShowWarningMessage(message.Message); break;
                case BoxState.Error: result = ShowErrorMessage(message.Message); break;
                default: return;
            }

            message.Reply(result);
        }

        public MessageBoxResult ShowErrorMessage(string message)
        {
            return HandyControl.Controls.MessageBox.Show(message, null, MessageBoxButton.YesNo, MessageBoxImage.Error);
        }

        public MessageBoxResult ShowMessage(string message)
        {
            return HandyControl.Controls.MessageBox.Show(message, null, MessageBoxButton.YesNo);
        }

        public MessageBoxResult ShowWarningMessage(string message)
        {
            return HandyControl.Controls.MessageBox.Show(message, null, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
}