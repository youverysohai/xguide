using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.Service
{
    public interface ICalibrationService
    {
        /// <summary>
        /// Performs a 9-point calibration for an Eye-in-Hand 2D configuration.
        /// </summary>
        /// <param name="calibration">The calibration view model containing the necessary parameters.</param>
        void EyeInHand2DConfig_Calibrate(CalibrationViewModel calibration);
    }
}