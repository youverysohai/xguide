using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using XGuideSQLiteDB.Models;
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
            var calibration = item as CalibrationViewModel;
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
