using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{

    public class UserViewModel : ViewModelBase, ICloneable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public SecureString PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Role { get; set; }
        public object Clone()
        {
            return new UserViewModel
            {
                Id = Id,
                Username = Username,
                Email = Email,
                PasswordHash = PasswordHash,
                IsActive = IsActive,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Role = Role
            };
        }

        public UserViewModel() { }
    }
}
