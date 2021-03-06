﻿using System;

namespace VirtualMethodTest
{
    class Program
    {
        static void Main(string[] args)
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
