using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace X_Vision.Converter
{
    public static class NavigationViewItemExtensions
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(NavigationViewItemExtensions), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(NavigationViewItemExtensions), new PropertyMetadata(null));


        public static readonly DependencyProperty BoolProperty =
            DependencyProperty.RegisterAttached("Bool", typeof(bool), typeof(NavigationViewItemExtensions), new PropertyMetadata(false));

        public static ICommand GetCommand(NavigationViewItem item)
        {
            return (ICommand)item.GetValue(CommandProperty);
        }

        public static void SetCommand(NavigationViewItem item, ICommand value)
        {
            item.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(NavigationViewItem item)
        {
            return item.GetValue(CommandParameterProperty);
        }

        public static bool GetBool(NavigationViewItem item)
        {
            return (bool)item.GetValue(BoolProperty);
        }

        public static void SetBool(NavigationViewItem item, bool value)
        {
            item.SetValue(BoolProperty, value);
        }

        public static void SetCommandParameter(NavigationViewItem item, object value)
        {
            item.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as NavigationViewItem;
            MouseButtonEventHandler previewMouseDownHandler = (s, args) => ExecuteCommand(item);

            if (item != null)
            {
                if (!GetBool(item))
                {
                    item.PreviewMouseDown += previewMouseDownHandler;
                    SetBool(item, true);
                }
            }
        }

        private static void ExecuteCommand(NavigationViewItem item)
        {

            var command = GetCommand(item);
            var commandParameter = GetCommandParameter(item);
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }


    }

}
