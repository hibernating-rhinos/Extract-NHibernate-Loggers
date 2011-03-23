using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace NHibernateLoggerExtractor
{
    public class ExtractLoggers
    {
        private readonly string _sourceFileLocation;

        public ExtractLoggers(string sourceFileLocation)
        {
            _sourceFileLocation = sourceFileLocation;
            var keepReferenceToNhibernate = new NHibernate.ADOException();
        }

        public IList<Logger> Get()
        {
            var loggers = new List<Logger>();

            var module = ModuleDefinition.ReadModule("NHibernate.dll");
            var classes = module.Types
                .Where(x => x.IsClass && x.HasFields)
                .ToList();

            var fields = classes
                .SelectMany(x => x.Fields)
                .Where(x => x.FieldType.FullName == "NHibernate.IInternalLogger" && x.IsStatic)
                .ToList();

            Console.WriteLine(string.Format("fields count: {0}", fields.Count));

            foreach (var fieldDefinition in fields)
            {
                var ctors = fieldDefinition.DeclaringType.Methods
                    .Where(x => x.IsConstructor && x.HasBody)
                    .Select(x => x.Body);

                foreach (var ctor in ctors)
                {
                    var a = ctor;
                }

                loggers.Add(new Logger
                                {
                                    ClassName = fieldDefinition.DeclaringType.FullName,
                                    LoggerType =  "",
                                    FieldName = fieldDefinition.Name,
                                });
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