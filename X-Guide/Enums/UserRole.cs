using System.ComponentModel;

namespace X_Guide.Enums
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