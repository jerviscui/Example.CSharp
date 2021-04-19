using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MethodDecorator.Fody.Interfaces;

namespace MethodDecoratorTest
{
    public class LogAttribute : Attribute, IMethodDecorator
    {
        public uint MaximumLength { get; set; }

        public uint MinimumLength { get; set; }

        public LogAttribute(uint maximumLength, uint minimumLength)
        {
            MaximumLength = maximumLength;
            MinimumLength = minimumLength;
        }

        /// <inheritdoc />
        public void Init(object instance, MethodBase method, object[] args)
        {
            Console.WriteLine("Init");
        }

        /// <inheritdoc />
        public void OnEntry()
        {
            Console.WriteLine("OnEntry");
        }

        /// <inheritdoc />
        public void OnExit()
        {
            Console.WriteLine("OnExit");
        }

        /// <inheritdoc />
        public void OnException(Exception exception)
        {
            Console.WriteLine("OnException");
        }
    }
}
