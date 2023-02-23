using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace X_Guide.MVVM.Command
{
    internal abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        //tells if a command can execute; if return false control is disabled
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        public void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        


    }
}
