using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;

namespace NHibernateLoggerExtractor
{
    public class ExtractLoggers
    {
        private readonly string _sourceFileLocation;

        public ExtractLoggers(string sourceFileLocation)
        {
            _sourceFileLocation = sourceFileLocation;
        }

        public IList<Logger> Get()
        {
            var loggers = new List<Logger>();

            var file = @"C:\OpenSourceProjects\NHibernate\nhibernate\src\NHibernate\Impl\AbstractSessionImpl.cs";
            using (IParser parser = ParserFactory.CreateParser(file))
            {
                parser.Parse();
                var visitor = new LoggerFieldVisitor();
                parser.CompilationUnit.AcceptVisitor(visitor, null);
                loggers.AddRange(visitor.Loggers);
            }
            return loggers;
        }

        private IList<string> GetAllCodeFilse()
        {
            DirectoryInfo directory = new DirectoryInfo(_sourceFileLocation);
            FileInfo[] files = directory.GetFiles("*.cs", SearchOption.AllDirectories);

            return files.Select(x => x.FullName).ToList();
        }
    }
}