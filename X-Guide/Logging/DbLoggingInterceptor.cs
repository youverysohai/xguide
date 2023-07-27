using Castle.DynamicProxy;
using System.Collections.Generic;

namespace X_Guide.Logging
{
    internal class DbLoggingInterceptor : IInterceptor
    {
        private readonly ILoggerFactory _logger;
        private readonly List<string> FilterMethod = new List<string> { "GetAll", "Find", "GetById" };

        public DbLoggingInterceptor(ILoggerFactory logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            if (!FilterMethod.Contains(invocation.Method.Name))
            {
                var logger = _logger.CreateLogger(invocation.InvocationTarget.GetType().Name);
                logger.Information($"{invocation.Method.Name}: {invocation.Arguments[0]}");
            }
        }
    }
}