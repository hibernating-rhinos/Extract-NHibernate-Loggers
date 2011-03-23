using System;
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

    public class ExposeTypeLoggerFactory : ILoggerFactory
    {
        public IInternalLogger LoggerFor(string keyName)
        {
            return new ExposeTypeLogger(keyName);
        }

        public IInternalLogger LoggerFor(Type type)
        {
            return LoggerFor(type.FullName);
        }
    }
}