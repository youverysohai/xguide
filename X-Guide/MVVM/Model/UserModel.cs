using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.Model
{
    public class UserModel
    {
        public int Id { get; set; }

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
          
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
        
            }
        }

        private int _role;

        public int Role
        {
            get { return _role; }
            set
            {
                _role = value;

            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
         
            }
        }

        private DateTime _createdAt;

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set
            {
                _createdAt = value;
            }
        }

        private DateTime _updatedAt;

        public DateTime UpdatedAt
        {
            get { return _updatedAt; }
            set
            {
                _updatedAt = value;
            }
        }

        public UserModel(string username, string email)
        {

            Username = username;
            Email = email;
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
