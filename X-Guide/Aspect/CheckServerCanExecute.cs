
using MethodDecorator.Fody.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Guide.Aspect
{
    public class CheckServerCanExecute
        : Attribute, IMethodDecorator
    {
        private readonly string _name;
        private bool _flag;

        public CheckServerCanExecute(string name)
        {
            _name = name;
        }

        public void Init(object instance, MethodBase method, object[] args)
        {
            _flag = (bool)instance.GetType().GetField(_name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);

        }

        public void OnEntry()
        {
            if (!_flag) throw new InvalidOperationException();
        }

        public void OnException(Exception exception)
        {
        }

        public void OnExit()
        {
        }
    }
}