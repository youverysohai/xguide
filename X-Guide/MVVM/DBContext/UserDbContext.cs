using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.DTOs;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.DBContext
{
    internal class UserDbContext : DbContext
    {
        public DbSet<UserDTO> Users { get; set; }
    }
}
