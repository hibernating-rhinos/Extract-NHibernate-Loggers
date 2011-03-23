using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateLoggerExtractor;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var extracter = new ExtractLoggers();
            var loggers = extracter.GetStaticLoggers();

            Console.WriteLine(string.Format("fields count: {0}", loggers.Count));

            foreach (var logger in loggers)
            {
                
            }
        }
    }
}
