using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace X_Guide.MVVM.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
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
            try
            {
                await _execute(parameter);
            }
            catch (Exception ex)
            {
            }
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}