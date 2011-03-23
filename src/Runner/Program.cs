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
            var path = @"C:\OpenSourceProjects\NHibernate\nhibernate\src";
            var parser = new ExtractLoggers(path);

            foreach (var a in parser.Get())
            {
                Console.WriteLine(a);
            }
        }
    }
}
