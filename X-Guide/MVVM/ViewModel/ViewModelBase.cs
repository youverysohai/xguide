using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected volatile bool _canDisplayViewModel = true;
        protected volatile bool _isLoaded = false;
        public virtual bool ReadyToDisplay() {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
        }
    }
}
