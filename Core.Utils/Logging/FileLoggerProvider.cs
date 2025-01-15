using Microsoft.Extensions.Logging;

namespace Core.Utils.Logging
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
}
