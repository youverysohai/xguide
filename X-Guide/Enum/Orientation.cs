using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide
{
    public enum Orientation
    {
        [Description("Look Downward")]
        LookDownward = 1,
        [Description("Look Upward")]
        LookUpward = 2,
        [Description("Eye On Hand")]
        EyeOnHand = 3,
        [Description("Mounted On Joint 2")]
        MountedOnJoint2 = 4,
        [Description("Mounted On Joint 5")]
        MountedOnJoint5 = 5,
    }
}
