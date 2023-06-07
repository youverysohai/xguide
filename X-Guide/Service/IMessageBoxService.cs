using System.Windows;

namespace X_Guide.Service
{
    internal interface IMessageBoxService
    {
        MessageBoxResult ShowMessage(string message);

        MessageBoxResult ShowWarningMessage(string message);

        MessageBoxResult ShowErrorMessage(string message);
    }
}