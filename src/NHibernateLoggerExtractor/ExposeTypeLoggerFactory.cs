using System;
using NHibernate;

namespace NHibernateLoggerExtractor
{
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