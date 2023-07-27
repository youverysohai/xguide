using Serilog;
using System;

namespace X_Guide.Logging
{
    internal interface ILoggerFactory
    {
        ILogger CreateLogger(string type);
    }

    public class SeriLogLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(string type) =>
            new LoggerConfiguration().WriteTo.File($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/X-Guide/logs/{type}.txt").CreateLogger();
    }
}