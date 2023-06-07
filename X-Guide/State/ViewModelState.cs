using CommunityToolkit.Mvvm.Messaging;
using System;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.State
{
    public class StateViewModel : ViewModelBase, IRecipient<ConnectionStatusChanged>, IRecipient<LoadingState>
    {
        public StateViewModel(IMessenger messenger)
        {
            messenger.RegisterAll(this);
        }

        void IRecipient<ConnectionStatusChanged>.Receive(ConnectionStatusChanged message)
        {
            IsAnyClientConnected = message.Value;
        }

        void IRecipient<LoadingState>.Receive(LoadingState message)
        {
            throw new NotImplementedException();
        }

        public bool IsAnyClientConnected { get; set; } = false;

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