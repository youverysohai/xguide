using MethodBoundaryAspect.Fody.Attributes;
using System.Diagnostics;
using System.Windows;

namespace X_Guide.Aspect
{
    internal class ApplicationRestartAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs arg)
        {
            // Start a new instance of the application
            Process.Start(Application.ResourceAssembly.Location);

            // Shutdown the current instance of the application
            Application.Current.Shutdown();
        }
    }
}