using System;

namespace X_Guide.MVVM.ViewModel
{
    public interface ICalibrationStep
    {
        void Register(Action action);

        void RegisterStateChange(Action<bool> action);
    }
}