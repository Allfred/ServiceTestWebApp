using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace CartService.Logging
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private static object _lock = new();
        public FileLogger(string path)
        {
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
 
        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }
 
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    var content = DateTime.Now + " " + logLevel+": "+ formatter(state, exception) + Environment.NewLine;
                    File.AppendAllText(filePath,content);
                }
            }
        }
    }
}