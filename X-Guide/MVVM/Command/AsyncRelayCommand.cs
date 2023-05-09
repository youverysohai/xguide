using System;
using System.Threading.Tasks;

namespace X_Guide.MVVM.Command
{
    internal class AsyncRelayCommand : RelayCommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null) : base(null, canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
    }
}