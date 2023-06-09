﻿using AutoMapper;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class UserManagementViewModel : ViewModelBase
    {
        public ObservableCollection<UserViewModel> Users { get; } = new ObservableCollection<UserViewModel>();


        private readonly IUserDb _userDb;
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


        public UserManagementViewModel(IUserDb userDb, IMapper mapper)
        {
            _userDb = userDb;
            _auth = new AuthenticationService(userDb);
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

        private async void AddUser(object obj)
        {

            bool success = await _auth.Register(new UserModel
            {
                Username = User.Username,
                Email = User.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = User.Role,
            }, InputPassword); ;
            if (success)
            {
                MessageBox.Show("Added successfully");
            }
            else MessageBox.Show("User is not added!");
            GetUsers();
        }

        private async void RemoveUser(object obj)
        {
            await _userDb.Delete(_mapper.Map<UserModel>(User));

            GetUsers();
        }

        private async void GetUsers()
        {
            IEnumerable<UserModel> models = await _userDb.GetAll();
            User = null;
            Users.Clear();

            foreach (var model in models)
            {
                Users.Add(_mapper.Map<UserViewModel>(model));
            }
        }

        private async void SaveUser(object obj)
        {
            User.UpdatedAt = DateTime.Now;
            bool saveStatus = await _userDb.Update(_mapper.Map<UserModel>(User));
            if (saveStatus)
            {
                System.Windows.MessageBox.Show("Saved successfully");
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to save User!");
            }

            GetUsers();
        }

        private void OnUserChangeEvent(object obj)
        {
            User = ((UserViewModel)obj).Clone() as UserViewModel;
        }
    }
}