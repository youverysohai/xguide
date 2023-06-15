using AutoMapper;
using ModernWpf.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows;
using X_Guide.MVVM.Command;
using X_Guide.Service;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.MVVM.ViewModel
{
    public class UserManagementViewModel : ViewModelBase
    {
        public ObservableCollection<UserViewModel> Users { get; } = new ObservableCollection<UserViewModel>();

        private readonly IMapper _mapper;

        public RelayCommand OpenUserFormCommand { get; }

        public RelayCommand SaveUserCommand { get; }
        public RelayCommand AddUserCommand { get; }
        public RelayCommand RemoveUserCommand { get; }
        public RelayCommand SelectedUserCommand { get; }
        public UserViewModel User { get; set; }

        private SecureString _inputPassword;

        private readonly AuthenticationService _auth;

        public SecureString InputPassword
        {
            private get { return _inputPassword; }
            set
            {
                _inputPassword = value;
            }
        }

        public ObservableCollection<UserRole> UserRoles { get; } = new ObservableCollection<UserRole>(Enum.GetValues(typeof(UserRole)).Cast<UserRole>());

        public UserManagementViewModel(IMapper mapper, IRepository repository)
        {
            _auth = new AuthenticationService(repository);
            _mapper = mapper;
            SelectedUserCommand = new RelayCommand(OnUserChangeEvent);
            SaveUserCommand = new RelayCommand(SaveUser);
            RemoveUserCommand = new RelayCommand(RemoveUser);
            AddUserCommand = new RelayCommand(AddUser);
            OpenUserFormCommand = new RelayCommand(OpenNewUserForm);
            GetUsers();
        }

        private async void OpenNewUserForm(object obj)
        {
            User = new UserViewModel();

            if (obj is ContentDialog dialog) await dialog.ShowAsync();
        }

        private void AddUser(object obj)
        {
            bool success = _auth.Register(_mapper.Map<User>(User), InputPassword); ;
            if (success)
            {
                MessageBox.Show("Added successfully");
            }
            else MessageBox.Show("User is not added!");
            GetUsers();
        }

        private void RemoveUser(object obj)
        {
            _auth.Delete(User.Id);
            GetUsers();
        }

        private async void GetUsers()
        {
            //IEnumerable<UserModel> models = await _auth.GetAll();
            //User = null;
            //Users.Clear();

            //foreach (var model in models)
            //{
            //    Users.Add(_mapper.Map<UserViewModel>(model));
            //}
        }

        private async void SaveUser(object obj)
        {
            //User.UpdatedAt = DateTime.Now;
            //bool saveStatus = await _userDb.Update(_mapper.Map<UserModel>(User));
            //if (saveStatus)
            //{
            //    System.Windows.MessageBox.Show("Saved successfully");
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Failed to save User!");
            //}

            //GetUsers();
        }

        private void OnUserChangeEvent(object obj)
        {
            User = ((UserViewModel)obj).Clone() as UserViewModel;
        }
    }
}