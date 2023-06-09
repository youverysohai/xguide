using System.ComponentModel;

namespace X_Guide.Enums
{
    public enum VisionSoftware
    {
        [Description("Hik")]
        VisionMaster = 1,

        [Description("Halcon")]
        Halcon = 2,

        [Description("Smart Camera")]
        SmartCamera = 3,

        [Description("Others")]
        Others = 4,

    }
}