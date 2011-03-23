using NHibernate;

namespace NHibernateLoggerExtractor
{
    public class ExposeTypeLogger : NoLoggingInternalLogger
    {
        public string Logger { get; set; }

        public ExposeTypeLogger(string logger)
        {
            Logger = logger;
        }
    }
}