using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    //displaying the current viewmodel of the application


    public class MainViewModel : ViewModelBase
    {
        #region CLR properties
        private bool _isLoggedIn = false;

      

        private string _iconConnection;

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

        public ICommand NavigateCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand ServerCommand { get; }

        public ICommand RegisterCommand { get; }

        #region ToTrigger
        public string IconConnection
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }


        private AuthenticationService _auth;

        private readonly NavigationStore _navigationStore;




        public ICommand NavigateCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand ServerCommand { get; }
                OnPropertyChanged();
            }
        }
        private string _sConnectionColor;





        private string _connectionStatus;
        private ResourceDictionary _resourceDictionary;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private string _inputUsername;
        public string InputUsername
        {
            get { return _inputUsername; }
            set
            {
                _inputUsername = value;
                OnPropertyChanged();
            }
        }

        private SecureString _inputPassword;
        public SecureString InputPassword
        {
            private get { return _inputPassword; }
            set
            {
                _inputPassword = value;
                OnPropertyChanged();
            }
        }

        private readonly AuthenticationService _auth;

        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public UserModel CurrentUser => _auth.CurrentUser;
            
            _resourceDictionary = resourceDictionary;

        public MainViewModel(NavigationStore navigationStore, Dictionary<PageName, NavigationService> viewModels, IServerService serverService, IUserService userService)
        {
            _resourceDictionary = resourceDictionary;
            _auth = new AuthenticationService(userService);
            _auth.CurrentUserChanged += OnCurrentUserChanged;
            
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            RegisterCommand = new RelayCommand(Register);

            #region ToTrigger
            ConnectionStatus = "Disconnected!";
            IconConnection = "LanDisconnect";

            SConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
            ConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
            #endregion

        }

        private void CurrentUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private void OnCurrentUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            
        }
        
        private async void Register(object obj)
        {
            bool success = await _auth.Register(new UserModel
            {
                Username = "123",
                Email = "Akimoputo.DotCom",
                Role = 1,
            IconConnection = "LanDisconnect";

            SConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
            ConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();

            }, InputPassword);
            if (success)
            {
                MessageBox.Show("Added successfully");
        
        private async void Login(object parameter)
        {
            //bool status = await _auth.Login(Username, Password);
            //if (status) MessageBox.Show($"Welcome back! {_auth.CurrentUser.Username}");
            //else MessageBox.Show("Invalid login");
            if (IsLoggedIn == true)
            {
                IsLoggedIn = false;
            }
            else
            {
                IsLoggedIn = true;
            }
        {
            bool status = await _auth.Login(Username, Password);
            if (status) MessageBox.Show($"Welcome back! {_auth.CurrentUser.Username}");
            else MessageBox.Show("Invalid login");

        }



        private void ServerEventHandler(object sender, TcpListenerEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if ((sender as ServerService).getServerStatus())
                {
                    //SConnectionColor = ((SolidColorBrush)_resourceDictionary["ConnectedColor"]).ToString();
                }
                else
                {

                    //SConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
                }
            });
        }


        private void ClientEventHandler(object sender, TcpClientEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.TcpClient.Connected)
                {
                    ConnectionStatus = "Connected!";
                    //ConnectionColor = ((SolidColorBrush)_resourceDictionary["ConnectedColor"]).ToString();
                }
                else
                {
                    ConnectionStatus = "Disconnected!";
                    //ConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
                }
            });
        }

      
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }

}
