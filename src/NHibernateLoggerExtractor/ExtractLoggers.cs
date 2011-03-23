using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateLoggerExtractor
{
    public class ExtractLoggers
    {
        private readonly string _sourceFileLocation;

        public ExtractLoggers(string sourceFileLocation)
        {
            _sourceFileLocation = sourceFileLocation;
            InitNHibernate();
        }

        public IList<Logger> Get()
        {
            var loggers = new List<Logger>();

            var module = ModuleDefinition.ReadModule("NHibernate.dll");
            var classes = module.Types
                .Where(x => x.IsClass && x.HasFields && x.Name == "AbstractSessionImpl")
                .ToList();

            var fields = classes
                .SelectMany(x => x.Fields)
                .Where(x => x.FieldType.FullName == "NHibernate.IInternalLogger" && x.IsStatic)
                .ToList();

            Console.WriteLine(string.Format("fields count: {0}", fields.Count));

            foreach (var fieldDefinition in fields)
            {
                var logger = new Logger
                                 {
                                     ClassName = fieldDefinition.DeclaringType.FullName,
                                     LoggerType = "",
                                     FieldName = fieldDefinition.Name,
                                 };
                var ctors = fieldDefinition.DeclaringType.Methods
                    .Where(x => x.IsConstructor && x.HasBody)
                    .Select(x => x.Body);

                foreach (var ctor in ctors)
                {
                    var a = ctor.GetILProcessor();
                }

                var nhibernate = Type.GetType(logger.ClassName + ", NHibernate");
                var field = nhibernate.GetField(logger.FieldName);
                logger.LoggerType = (string)field.GetValue(null);

                loggers.Add(logger);
            }

            return loggers;
        }

        private void InitNHibernate()
        {
            var c = new Configuration()
                .Configure("NHibernate.config");
        }
    }
}