using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class UserManagementViewModel : ViewModelBase
    {
        public ObservableCollection<UserViewModel> Users { get; } = new ObservableCollection<UserViewModel>();


        private readonly IUserDb _userDb;
        private readonly IMapper _mapper;

        public RelayCommand SaveUserCommand { get;}

        public RelayCommand RemoveUserCommand { get;}
        public RelayCommand SelectedUserCommand { get;}
        public UserViewModel User { get; set; }
        public ObservableCollection<UserRole> UserRoles { get; } = new ObservableCollection<UserRole>(Enum.GetValues(typeof(UserRole)).Cast<UserRole>());


        public UserManagementViewModel(IUserDb userDb,IMapper mapper)
        {
            _userDb = userDb;
            _mapper = mapper;
            SelectedUserCommand = new RelayCommand(OnUserChangeEvent);
            SaveUserCommand = new RelayCommand(SaveUser);
            RemoveUserCommand = new RelayCommand(RemoveUser);
            GetUsers();
        }

        private void RemoveUser(object obj)
        {
            
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
