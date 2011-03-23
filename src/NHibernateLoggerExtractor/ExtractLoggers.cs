using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateLoggerExtractor
{
    public class ExtractLoggers
    {
        public IList<Logger> GetStaticLoggers()
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

                var type = Type.GetType(logger.ClassName + ", NHibernate");
                var field = type.GetField(logger.FieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);

                try
                {
                    var l = (ExposeTypeLogger)field.GetValue(null);
                    logger.LoggerType = l.Logger;
                }
                catch (Exception)
                {
                    continue;
                }

                loggers.Add(logger);
            }

            return loggers;
        }
    }
}