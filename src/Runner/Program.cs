using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NHibernateLoggerExtractor;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputPath = "loggers.txt";

            var extracter = new ExtractLoggers();
            var loggers = extracter.GetStaticLoggers();

            Console.WriteLine(string.Format("fields count: {0}", loggers.Count));

            var builder = new StringBuilder();
            foreach (var logger in loggers)
            {
                builder.AppendFormat("new {5}LoggerType = {0}{1}{0}, ClassName={0}{2}{0}, FieldName = {0}{3}{0}{6},{4}", "\"", logger.LoggerType, logger.ClassName, logger.FieldName, Environment.NewLine, "{", "}");
            }
            var text = builder.ToString();
            File.WriteAllText(outputPath, text, Encoding.UTF8);
        }
    }
}
