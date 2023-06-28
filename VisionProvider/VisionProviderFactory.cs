namespace VisionProvider
{
    public enum VisionSoftware
    {
        Hik,
        Halcon,
        SmartCamera,
        Others
    };

    public class VisionProviderFactory
    {
        public static IVisionService? Create(VisionSoftware software)
        {
            switch (software)
            {
                case VisionSoftware.Hik:
                    return null;

                case VisionSoftware.Halcon: return null;

                case VisionSoftware.SmartCamera: return null;
                case VisionSoftware.Others: return null;
                default: return null;
            }
        }
    }
}