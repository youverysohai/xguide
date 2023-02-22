using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.Model
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserModel(string username, string email, string passwordHash)
        {

            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }

        // Optional: additional properties or methods here

        // Required: default constructor
        public UserModel()
        {
            // Set default values as needed
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
