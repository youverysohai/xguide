using System;

namespace X_Guide.MVVM.ViewModel
{
    public interface ICalibrationStep
    {
        void Subscribe(EventHandler<int> action);

        void Unsubscribe(EventHandler<int> action);
    }
}