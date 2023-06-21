using MethodBoundaryAspect.Fody.Attributes;

//using ToastNotifications.Messages;

namespace X_Guide.Aspect
{
    public sealed class ExceptionHandlingAspect : OnMethodBoundaryAspect
    {
        private readonly FlowBehavior _flowBehavior;

        public ExceptionHandlingAspect()
        {
            _flowBehavior = FlowBehavior.Return;
        }

        public ExceptionHandlingAspect(FlowBehavior flowBehavior)
        {
            _flowBehavior = flowBehavior;
        }

        public override void OnException(MethodExecutionArgs arg)
        {
            //App.Notifier.ShowError(arg.Exception.Message);
            arg.FlowBehavior = _flowBehavior;
        }
    }
}