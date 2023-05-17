using System.Windows;

namespace X_Guide.Service
{
    internal class MessageService : IMessageService
    {
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