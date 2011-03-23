using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace NHibernateLoggerExtractor
{
    public class LoggerFieldVisitor : AbstractAstVisitor
    {
        public readonly List<Logger> Loggers = new List<Logger>();

        public override object VisitFieldDeclaration(FieldDeclaration fieldDeclaration, object data)
        {
            if (fieldDeclaration.TypeReference.Type.Contains("IInternalLogger"))
            {
                Loggers.Add(new Logger
                                {
                                    LoggerType = "",
                                    ClassName = "",
                                    FieldName = fieldDeclaration.Fields[0].Name,
                                });
            }

            base.VisitFieldDeclaration(fieldDeclaration, data);

            return null;
        }

        public override object VisitCompilationUnit(CompilationUnit compilationUnit, object data)
        {
            return null;
        }
    }
}