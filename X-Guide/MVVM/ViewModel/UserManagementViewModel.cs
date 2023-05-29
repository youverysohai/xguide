using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class UserManagementViewModel : ViewModelBase
    {
        public ObservableCollection<UserViewModel> Users { get; } = new ObservableCollection<UserViewModel>();


        private readonly IUserDb _userDb;
        private readonly IMapper _mapper;

        public UserManagementViewModel(IUserDb userDb,IMapper mapper)
        {
            _userDb = userDb;
            _mapper = mapper;

            GetUsers();
        }

        private async void GetUsers()
        {
            IEnumerable<UserModel> models = await _userDb.GetAll();
           // User = null;
            Users.Clear();

            foreach (var model in models)
            {
                Users.Add(_mapper.Map<UserViewModel>(model));
            }
        }
    }
}
