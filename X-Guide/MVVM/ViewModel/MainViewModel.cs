using System;
using System.Collections.Generic;
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

        public bool IsLoggedIn
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

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private SecureString _password;

        public SecureString Password
        {
            private get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        #endregion

        public MainViewModel(NavigationStore navigationStore, Dictionary<PageName, NavigationService> viewModels, IServerService serverService, ResourceDictionary resourceDictionary, IUserService userService)
        {
            _resourceDictionary = resourceDictionary;
            _auth = new AuthenticationService(userService);


            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            serverService.ClientEvent += ClientEventHandler;
            serverService.ListenerEvent += ServerEventHandler;
            NavigateCommand = new NavigateCommand(viewModels);
            LoginCommand = new RelayCommand(Login);
            /*    ServerCommand = new ConnectServerCommand(serverService);*/


        }

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
