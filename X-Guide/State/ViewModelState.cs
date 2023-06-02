using System;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.State
{
    public class ViewModelState : ViewModelBase
    {
        public ViewModelState(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<bool>(OnConnected);
        }

        public void OnConnected(bool connected)
        {
            isManipulatorConnected = connected;
        }

        public bool isManipulatorConnected = false;

        public Action OnStateChanged;

        public bool IsCalibValid { get; set; } = true;

        private bool _isLoading;
        private readonly IEventAggregator _eventAggregator;

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