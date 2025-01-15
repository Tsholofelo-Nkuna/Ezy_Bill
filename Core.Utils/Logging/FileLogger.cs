using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.Logging
{
    public class FileLogger : ILogger
    {
        public static object Lock { get; } = new object();
        private readonly string _category;
        public FileLogger(string category) {
          _category = category;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (FileLogger.Lock) { 
                var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{DateTime.Now:yyMMdd}.log";
                File.AppendAllLines(filePath, new[] { $"At {DateTime.Now:hh:mm tt} {_category} said:", exception?.Message ?? string.Empty, formatter(state, exception) });
            }
        }
    }
}
