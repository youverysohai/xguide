using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.Service
{
    public interface ICalibrationService
    {
        /// <summary>
        /// Performs a 9-point calibration for an Eye-in-Hand 2D configuration.
        /// </summary>
        /// <param name="calibration">The calibration view model containing the necessary parameters.</param>
        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset);

        Task<CalibrationData> EyeInHand2D_Calibrate(int XOffset, int YOffset, double XMove, double YMove);
    }
}