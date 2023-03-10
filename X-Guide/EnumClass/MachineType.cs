using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide
{
    public enum MachineType
    {
        [Description("Gantry System")]
        GantrySystem = 1,
        [Description("SCARA")]
        SCARA = 2,
        [Description("6-Axis")]
        SixAxis = 3,


    }
}
