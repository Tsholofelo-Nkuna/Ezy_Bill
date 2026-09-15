using Microsoft.Extensions.Logging;

namespace ClientManagement.Utils.Logging
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
