using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Enums
{
    public enum VisionSoftware
    {
        [Description("Vision Master")]
        VisionMaster = 1,
        [Description("HIK Smart Camera")]
        HikSmartCamera = 2,
        [Description("MVTec HALCON")]
        MVTecHALCON = 3
    }
}
