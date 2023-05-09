using MethodBoundaryAspect.Fody.Attributes;
using ToastNotifications.Messages;

namespace X_Guide.Aspect
{
    public sealed class ExceptionHandlingAspect : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs arg)
        {
            App.Notifier.ShowError(arg.Exception.Message);
        }
    }
}