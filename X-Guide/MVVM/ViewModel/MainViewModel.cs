using Autofac;
using Microsoft.Extensions.Logging;
using ModernWpf.Controls;
using System;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;
using X_Guide.State;

namespace X_Guide.MVVM.ViewModel
{
    //displaying the current viewmodel of the application

    public class MainViewModel : ViewModelBase
    {
        #region CLR properties

        public bool Test { get; set; } = false;
        public RelayCommand TestCommand { get; }
        public RelayCommand ChangeThemeCommand { get; }

        public bool IsRunning => State.IsLoading;
        public UserViewModel User { get; set; }
        private bool _isLoggedIn = false;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

        private bool _isBrightTheme = false;

        public bool IsBrightTheme
        {
            get { return _isBrightTheme; }
            set { _isBrightTheme = value; OnPropertyChanged(); }
        }


        public ICommand NavigateCommand { get; }

        public ICommand CurrentUserCommand { get; }
        public ICommand OpenLoginFormCommand { get; }
        public ICommand OpenRegisterFormCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public ICommand ServerCommand { get; }

        public ICommand RegisterCommand { get; }

        private string _inputUsername;

        public string InputUsername
        {
            get { return _inputUsername; }
            set
            {
                _inputUsername = value;
            }
        }
        private string _inputEmail;

        public string InputEmail
        {
            get { return _inputEmail; }
            set { _inputEmail = value; }
        }


        private SecureString _inputPassword;

        public SecureString InputPassword
        {
            private get { return _inputPassword; }
            set
            {
                _inputPassword = value;
            }
        }

        private string _currentUsername;

        public string CurrentUsername
        {
            get { return _currentUsername; }
            set { _currentUsername = value; OnPropertyChanged(); }
        }

        private string _currentUserRole;

        public string CurrentUserRole
        {
            get { return _currentUserRole; }
            set { _currentUserRole = value; OnPropertyChanged(); }
        }


        private bool _isManipulatorConnected;

        public bool IsManipulatorConnected
        {
            get { return _isManipulatorConnected; }
            set { _isManipulatorConnected = value; OnPropertyChanged(); }
        }

        private readonly IServerService _serverService;
        private readonly AuthenticationService _auth;
        public ViewModelState State { get; set; }
        private readonly INavigationService _navigationService;
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public UserModel CurrentUser => _auth.CurrentUser;

        #endregion CLR properties

        public MainViewModel(INavigationService navigationService, IServerService serverService, IUserDb userService, ILogger logger, ViewModelState state)
        {
            _auth = new AuthenticationService(userService);
            _auth.CurrentUserChanged += OnCurrentUserChanged;
            State = state;
            State.OnStateChanged = OnLoadingStateChanged;
            _navigationService = navigationService;
            _navigationStore = navigationService.GetNavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            serverService.Start();
            var nav = new TypedParameter(typeof(INavigationService), _navigationService);
            TestCommand = new RelayCommand(test);

            ChangeThemeCommand = new RelayCommand(ToggleTheme);

            _serverService = serverService;

            _serverService.SubscribeOnClientConnectionChange(OnConnectionChange);

            _navigationService.Navigate<SettingViewModel>();

            CurrentUserCommand = new RelayCommand(OnUserChangeEvent);
            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(Logout);
            RegisterCommand = new RelayCommand(Register);
            NavigateCommand = new RelayCommand(Navigate);
            logger.LogInformation("LapisLazuli");
        }


        private void OnUserChangeEvent(object obj)
        {
            User = ((UserViewModel)obj).Clone() as UserViewModel;
            if (User.IsActive)
            {
                User.IsActive = false;
                MessageBox.Show("Log out : " + _auth.CurrentUser.Username);
            }
            else
            {
                User.IsActive = true;
            }
        }

        private void Logout(object obj)
        {
            CurrentUsername = "";
            CurrentUserRole = "";
            _auth.CurrentUser.Equals(null);
        }

        private void OnCurrentUserChanged()
        {

            MessageBox.Show("Hi, new user");
        }

        private void ToggleTheme(object obj)
        {
            if (IsBrightTheme)
            {
                IsBrightTheme = false;
            }
            else
            {
                IsBrightTheme = true;
            }
        }
        private void OnConnectionChange(object sender, bool e)
        {
            IsManipulatorConnected = e;

        }

        private void OnLoadingStateChanged()
        {
            OnPropertyChanged(nameof(IsRunning));
        }

        private void test(object obj)
        {
            Test = !Test;
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
                case PageName.UserManagement: _navigationService.Navigate<UserManagementViewModel>(); break;
                case PageName.Operation: _navigationService.Navigate<OperationViewModel>(); break;
                case PageName.JogRobot: _navigationService.Navigate<JogRobotViewModel>(); break;
                default: break;
            }
        }

        private async void Register(object obj)
        {

                bool success = await _auth.Register(new UserModel
            {
                Username = InputUsername,
                Email = InputEmail,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = 1,
            }, InputPassword); ;
            if (success)
            {
                MessageBox.Show("Added successfully");
            }
            else MessageBox.Show("User is not added!");
        }

        private async void Login(object obj)
        {
            bool status = await _auth.Login(InputUsername, InputPassword);

            if (status)
            {
                MessageBox.Show($"Welcome back! {_auth.CurrentUser.Username}");
                CurrentUsername = _auth.CurrentUser.Username;
                CurrentUserRole = Enum.GetName(typeof(UserRole), _auth.CurrentUser.Role);
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

        public override void Dispose()
        {
            _serverService.UnsubscribeOnClientConnectionChange(OnConnectionChange);
            base.Dispose();
        }
    }
}
