using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using Orientation = X_Guide.Enums.Orientation;

namespace X_Guide.Extension
{
    public class TemplateSelector : DataTemplateSelector
    {
        public TemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            JogImplementationViewModel implementation = item as JogImplementationViewModel;
            CalibrationViewModel calibration = implementation?.Calibration;
            if (calibration is null) return null;

            var parent = container as FrameworkElement;

            switch (calibration.Orientation)
            {
                case Orientation.LookDownward: return (DataTemplate)parent.TryFindResource("TopConfigView");
                case Orientation.LookUpward:
                    return (DataTemplate)parent.TryFindResource("TopConfigView");

                default: return null;
            }
        }
    }
}