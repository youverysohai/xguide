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
    public static class NavigationViewItemExtensions 
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(NavigationViewItemExtensions), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(NavigationViewItemExtensions), new PropertyMetadata(null));

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

        public static void SetCommandParameter(NavigationViewItem item, object value)
        {
            item.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as NavigationViewItem;
            if (item != null)
            {
                item.PreviewMouseDoubleClick += (sender, args) =>
                {
                    var command = GetCommand(item);
                    var commandParameter = GetCommandParameter(item);
                    if (command != null && command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                };
            }
        }
    }

}
