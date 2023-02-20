//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



//namespace X_Guide
//{
//    public class FileLogger : ILogger
//    {
//        private string filePath;
//        private static object _lock = new object();
//        private string path;

//        public FileLogger()
//        {
            
//        }

//        public FileLogger(string path)
//        {
//            filePath = path;
//        }

//        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
//        {
//            return null;
//        }

//        public bool IsEnabled(LogLevel logLevel)
//        {
//            return true;
//        }

//        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
//        {
//            if (formatter != null)
//            {
//                lock (_lock)
//                {
//                    string fullFilePath = Path.Combine(filePath, DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
//                    var n = Environment.NewLine;
//                    string exc = "";
//                    if (exception != null) exc = n + exception.GetType() + ": " + exception.Message + n + exception.StackTrace + n;
//                    File.AppendAllText(fullFilePath, logLevel.ToString() + ": " + DateTime.Now.ToString() + " " + formatter(state, exception) + n + exc);
//                }
//            }
//        }
//    }
//}
