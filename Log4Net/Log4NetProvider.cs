using Microsoft.Extensions.Logging;

namespace webapisample.Log4Net
{
    public class Log4NetProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new Log4NetLogger(categoryName);
        }

        public void Dispose()
        {
        }
    }
}