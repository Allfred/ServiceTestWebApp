using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace CartService.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;
        public FileLoggerProvider(string directory, string fileName)
        {
            _path = Path.Combine(directory, "Logs");

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            _path = Path.Combine(_path, fileName);
            
            File.WriteAllText(_path, String.Empty);
            
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }
 
        public void Dispose()
        {
        }
    }
}