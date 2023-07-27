using System;
using System.ComponentModel.DataAnnotations;

namespace XGuideSQLiteDB.Models
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public Nullable<int> Role { get; set; }

        public override string ToString()
        {
            return $"User [Id: {Id}, Username: {Username}, Email: {Email}, IsActive: {IsActive}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, Role: {Role}]";
        }
    }
}