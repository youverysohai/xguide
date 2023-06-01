using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 0,
        [Description("Operator")]
        Operator = 1,
        [Description("Guest")]
        Guest = 2,
    }
}
