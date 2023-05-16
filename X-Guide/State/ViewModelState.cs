using System;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.State
{
    public class ViewModelState : ViewModelBase
    {
        public Action OnStateChanged;

        public bool IsCalibValid { get; set; } = true;

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnStateChanged?.Invoke();
            }
        }
    }
}