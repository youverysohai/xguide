using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
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
            set { _isLoggedIn = value; }
        }

        public ICommand NavigateCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand ServerCommand { get; }

        public ICommand RegisterCommand { get; }


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

        private INavigationService _navigationService;
        private NavigationStore _navigationStore;


        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public UserModel CurrentUser => _auth.CurrentUser;
        #endregion

        public MainViewModel(INavigationService navigationService, IServerService serverService, IUserDb userService)
        {

            _auth = new AuthenticationService(userService);
            _auth.CurrentUserChanged += OnCurrentUserChanged;
            
            _navigationService = navigationService;

            _navigationStore = navigationService.GetNavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            serverService.StartServer();
            var nav = new TypedParameter(typeof(INavigationService), _navigationService);

         
            _navigationService.Navigate<SettingViewModel>(nav);
   
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            NavigateCommand = new RelayCommand(Navigate);

        }

        private void Navigate(object obj)
        {
            var nav = new TypedParameter(typeof(INavigationService), _navigationService);
           
            switch (obj)
            {

                case PageName.Security: _navigationService.Navigate<SecurityViewModel>(); break;
                case PageName.Production: _navigationService.Navigate<ProductionViewModel>(); break;
                case PageName.Setting: _navigationService.Navigate<SettingViewModel>(); break;
                case PageName.CalibrationWizardStart: _navigationService.Navigate<CalibrationWizardStartViewModel>(nav); break;
                case PageName.Login: _navigationService.Navigate<UserLoginViewModel>(); break;
                default: break;
            }
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
            MessageBox.Show("Halo chub");
            /*bool success = await _auth.Register(new UserModel
            {
                Username = "123",
                Email = "Akimoputo.DotCom",
                Role = 1,

            }, InputPassword);
            if (success)
            {
                MessageBox.Show("Added successfully");

            }
            else MessageBox.Show("User is not added!");*/
        }



        private async void Login(object obj)
        {
            bool status = await _auth.Login(InputUsername, InputPassword);

            if (status)
            {
                MessageBox.Show($"Welcome back! {_auth.CurrentUser.Username}");
            }
            else
            {
                MessageBox.Show("Invalid login");
            }

        }




        private void OnCurrentViewModelChanged()
        {
  
            OnPropertyChanged(nameof(CurrentViewModel));
            Debug.WriteLine(CurrentViewModel);

        }

    }

}
