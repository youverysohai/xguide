using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace X_Guide.MVVM.ViewModel
{
    //displaying the current viewmodel of the application

    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private string _iconConnection;

        public string IconConnection
        {
            get { return _iconConnection; }
            set { _iconConnection = value;

                OnPropertyChanged();
            }

        }

        private string _connectionColor;

        public ICommand NavigateCommand { get; set; }

        public ICommand ServerCommand { get; set; }
        public string ConnectionColor
        {
            get { return _connectionColor; }
            set
            {
                _connectionColor = value;
                OnPropertyChanged();
            }
        }
        private string _sConnectionColor;

        public string SConnectionColor
        {
            get { return _sConnectionColor; }
            set
            {
                _sConnectionColor = value;
                OnPropertyChanged();
            }
        }



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



        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        public MainViewModel(NavigationStore navigationStore, Dictionary<PageName, NavigationService> viewModels, IServerService serverService, ResourceDictionary resourceDictionary)
        {

            _resourceDictionary = resourceDictionary;



            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            serverService.ClientEvent += ClientEventHandler;
            serverService.ListenerEvent += ServerEventHandler;
            NavigateCommand = new NavigateCommand(viewModels);
        /*    ServerCommand = new ConnectServerCommand(serverService);*/
            ConnectionStatus = "Disconnected!";
            IconConnection = "LanDisconnect";

            SConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
            ConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();


        }


        private void ServerEventHandler(object sender, TcpListenerEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if ((sender as ServerService).getServerStatus())
                {
                    SConnectionColor = ((SolidColorBrush)_resourceDictionary["ConnectedColor"]).ToString();
                }
                else
                {
                    
                    SConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
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
                    ConnectionColor = ((SolidColorBrush)_resourceDictionary["ConnectedColor"]).ToString();
                }
                else
                {
                    ConnectionStatus = "Disconnected!";
                    ConnectionColor = ((SolidColorBrush)_resourceDictionary["DisconnectedColor"]).ToString();
                }
            });
        }





        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }

}
