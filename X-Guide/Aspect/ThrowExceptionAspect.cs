using MethodBoundaryAspect.Fody.Attributes;

namespace X_Guide.Aspect
{
    public class ThrowExceptionAspect : OnMethodBoundaryAspect
    {
        private readonly string _message = string.Empty;

        public ThrowExceptionAspect()
        {
        }

        public ThrowExceptionAspect(string message)
        {
            _message = message;
        }

        public override void OnException(MethodExecutionArgs arg)
        {
        }
    }
}