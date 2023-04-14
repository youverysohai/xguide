using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Enums

{
    public enum ManipulatorType
    {
        [Description("Gantry With Rotary Axis")]
        GantrySystemR = 1,
        [Description("Gantry Without Rotary Axis")]
        GantrySystemWR = 2,
        [Description("SCARA")]
        SCARA = 3,
        [Description("6-Axis")]
        SixAxis = 4,
        

    }
}
