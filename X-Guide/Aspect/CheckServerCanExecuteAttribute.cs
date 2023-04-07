
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Guide.Aspect
{
    [Serializable]
    public class CheckServerCanExecuteAttribute : OnMethodBoundaryAspect
    {
        private readonly string _name;

        public CheckServerCanExecuteAttribute(string name)
        {
            _name = name;
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            object instance = args.Instance;
            bool CanExecute = (bool)instance.GetType().GetField(_name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
            if (!CanExecute) throw new Exception("Chun bobo");

        }
    }
}
