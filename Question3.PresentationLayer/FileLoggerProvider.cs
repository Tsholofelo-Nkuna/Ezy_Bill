namespace Question3.PresentationLayer
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
           return new FileLogger(categoryName);
        }

        public void Dispose()
        {
           
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private static readonly object _lock = new object();
        public FileLogger(string categoryName) { 
            this._categoryName = categoryName;
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
            lock (_lock)
            {
                File.AppendAllLines(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Now.ToString("yyMMdd")}.log"),
                    new string[]
                    {
                        $"At {DateTime.Now.ToString("yyyy MMM dd | hh:mm:ss")} {this._categoryName} said",
                        formatter(state, exception),
                        $"Exception Message: {exception?.Message}",
                        $"Exception Stack Trace:",
                        exception?.StackTrace ?? string.Empty,
                    }
                 );
            }
        }
    }
}
