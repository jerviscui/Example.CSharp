using System;
using System.Reflection;
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
