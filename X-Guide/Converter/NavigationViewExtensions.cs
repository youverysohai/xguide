using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace X_Guide.Converter
{
    public static class NavigationViewExtensions
    {
        public static readonly DependencyProperty SettingsCommandProperty =
            DependencyProperty.RegisterAttached("SettingsCommand", typeof(ICommand), typeof(NavigationViewExtensions), new PropertyMetadata(null, OnSettingsCommandPropertyChanged));

        public static readonly DependencyProperty SettingsCommandParameterProperty =
            DependencyProperty.RegisterAttached("SettingsCommandParameter", typeof(object), typeof(NavigationViewExtensions), new PropertyMetadata(null));
        
        public static ICommand GetSettingsCommand(NavigationView navigationView)
        {
            return (ICommand)navigationView.GetValue(SettingsCommandProperty);
        }

        public static void SetSettingsCommand(NavigationView navigationView, ICommand value)
        {
            navigationView.SetValue(SettingsCommandProperty, value);
        }

        public static object GetSettingsCommandParameter(NavigationView navigationView)
        {
            return navigationView.GetValue(SettingsCommandParameterProperty);
        }

        public static void SetSettingsCommandParameter(NavigationView navigationView, object value)
        {
            navigationView.SetValue(SettingsCommandParameterProperty, value);
        }

        private static void OnSettingsCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navigationView = (NavigationView)d;
            //navigationView.Drop += (sender, args) =>
            //{
            //    var command = GetSettingsCommand(navigationView);
            //    var commandParameter = GetSettingsCommandParameter(navigationView);

            //    if (command != null && command.CanExecute(commandParameter))
            //    {
            //        command.Execute(commandParameter);
            //    }
            //};
        }
    }
}
