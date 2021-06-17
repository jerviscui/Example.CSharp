using System;

namespace StringFormatTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public static string UnBoxTest()
        {
            int a = 1;

            //编译器提示 IDE0071 简化内插
            return $"string {a.ToString()}";

            //IL_0003: ldstr        "string "
            //IL_0008: ldloca.s     a
            //IL_000a: call         instance string [System.Runtime]System.Int32::ToString()
            //IL_000f: call         string [System.Runtime]System.String::Concat(string, string)
        }

        public static string BoxTest()
        {
            int a = 1;

            return $"string {a}";

            //IL_0003: ldstr        "string {0}"
            //IL_0008: ldloc.0      // a
            //IL_0009: box          [System.Runtime]System.Int32
            //IL_000e: call         string [System.Runtime]System.String::Format(string, object)
        }
    }
}
