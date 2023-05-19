using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected volatile bool _canDisplayViewModel = true;
        protected volatile bool _isLoaded = false;

        public virtual Task<bool> ReadyToDisplay()
        {
            return Task.FromResult(true);
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