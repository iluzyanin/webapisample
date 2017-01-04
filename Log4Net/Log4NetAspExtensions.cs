using Microsoft.Extensions.Logging;

namespace webapisample.Log4Net
{
    public static class Log4NetAspExtensions
    {
        public static void AddLog4Net(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new Log4NetProvider());
        }
    }
}