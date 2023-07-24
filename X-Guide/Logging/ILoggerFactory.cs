using Serilog;
using System;

namespace X_Guide.Logging
{
    internal interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }

    public class SeriLogLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type) =>
            new LoggerConfiguration().WriteTo.File($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/X-Guide/logs/{type}.txt").CreateLogger();
    }
}