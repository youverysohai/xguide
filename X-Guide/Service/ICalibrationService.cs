using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service
{
    public interface ICalibrationService
    {
        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, int JointRotationAngle);

        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove);
    }
}