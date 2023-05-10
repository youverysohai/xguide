using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace X_Guide.MVVM.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter = null)
        {
            _execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public static RelayCommand FromAsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            return new RelayCommand(async (parameter) =>
            {
                try
                {
                    await execute(parameter);
                }
                catch { }
            }, canExecute);
        }
    }
}