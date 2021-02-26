using System;

namespace DelegateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MethodDelegate.DelegatePerformanceTest();

            MethodDelegate.StaticMethodTest();

            MethodDelegate.ExpressionTest();
        }
    }
}
