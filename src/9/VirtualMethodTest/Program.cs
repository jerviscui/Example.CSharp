using System;

namespace VirtualMethodTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var t = new VirtualMethodTest();
            t.OverrideEmptyMethodTest();
            t.OverrideMethodTest();
            t.CovariantExecMethodTest();
            t.ChangeRuntimeTypeAndCovariantExecMethodTest();
            t.NoInlineTest();
            t.InterfaceExecTest();

            Console.ReadKey();
        }
    }
}
