using System.Windows;
using System.Windows.Controls;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.View
{
    public class ViewTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IVisionViewModel visionView)
            {
                if (visionView is HalconViewModel)
                    return (DataTemplate)Application.Current.Resources["HalconViewTemplate"];
                else if (visionView is HikViewModel)
                    return (DataTemplate)Application.Current.Resources["HikViewTemplate"];
            }
            return base.SelectTemplate(item, container);
        }
    }
}