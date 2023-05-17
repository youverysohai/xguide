using System.Windows;

namespace X_Guide.Service
{
    internal interface IMessageService
    {
        MessageBoxResult ShowMessage(string message);

        MessageBoxResult ShowWarningMessage(string message);

        MessageBoxResult ShowErrorMessage(string message);
    }
}