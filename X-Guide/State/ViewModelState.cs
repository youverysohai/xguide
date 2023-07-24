using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using System;
using TcpConnectionHandler;
using X_Guide.MessageToken;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;
using XGuideSQLiteDB.Models;

namespace X_Guide.State
{
    public class StateViewModel : ViewModelBase, IRecipient<ConnectionStatusChanged>, IRecipient<LoadingState>, IRecipient<ClientStatusChanged>, IRecipient<UserLoginChanged>
    {
        public StateViewModel(IMessenger messenger, IMapper mapper)
        {
            messenger.RegisterAll(this);
            _mapper = mapper;
        }

        void IRecipient<ConnectionStatusChanged>.Receive(ConnectionStatusChanged message)
        {
            IsAnyClientConnected = message.Value;
        }

        void IRecipient<LoadingState>.Receive(LoadingState message)
        {
            throw new NotImplementedException();
        }

        void IRecipient<ClientStatusChanged>.Receive(ClientStatusChanged message)
        {
            IsConnectedToServer = message.Value;
        }

        void IRecipient<UserLoginChanged>.Receive(UserLoginChanged message)
        {

            CurrentUser = _mapper.Map<UserViewModel>(message.Value);
            _ = CurrentUser != null ? IsLoggedIn = true : IsLoggedIn = false;
            
        }
        
        public UserViewModel CurrentUser { get; private set; }
        public bool IsLoggedIn { get; set; } = false;

        public bool IsAnyClientConnected { get; set; } = false;

        public Action OnStateChanged;

        public bool IsConnectedToServer { get; set; } = false;
        public bool IsCalibValid { get; set; } = true;

        private bool _isLoading;
        private readonly IMapper _mapper;

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