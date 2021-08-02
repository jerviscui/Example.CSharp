using System;
using System.Threading.Tasks;

namespace ValueTupleTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //赋值给一个元组对象
            var result = ReturnValueTuple_Test();
            Console.WriteLine($"{result.First} {result.Last}");

            //以元组结构接收返回值
            (string first, string last) = ReturnValueTuple_Test();
            Console.WriteLine($"{first} {last}");

            var compare = (First: "first", Last: "last");
            Console.WriteLine(result == compare);

            Console.WriteLine();
            var r = await Async_Test();
            Console.WriteLine(r);

            Console.WriteLine();
            Assignment_Test();

            Console.WriteLine();
            ClassDeconstruction_Test();
        }

        private static (string First, string Last) ReturnValueTuple_Test()
        {
            var names = ("first", "last");
            return names;

#pragma warning disable CS0162 // 检测到无法访问的代码
            var names2 = (First: "first", Last: "last");
            return names2;

            (string First, string Last) names3 = ("first", "last");
            return names3;
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        private static Task<(string First, int Last)> Async_Test()
        {
            return Task.FromResult(("a", 1));
        }

        private static void Assignment_Test()
        {
            var unnamed = (42, "The meaning of life");
            var anonymous = (16, "a perfect square");
            var named = (Answer: 42, Message: "The meaning of life");
            var differentNamed = (SecretConstant: 40, Label: "The different meaning of life");

            //named to unnamed:
            unnamed = named;

            // unnamed to named:
            named = anonymous;
            Console.WriteLine($"{named.Answer}, {named.Message}");

            // different name assignment:
            named = differentNamed;
            Console.WriteLine($"{named.Answer}, {named.Message}");

            // With implicit conversions:
            // int can be implicitly converted to long
            (long, string) conversion = named;
        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
        private static void Deconstruction_Test()
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            (string first1, string last1) = ReturnValueTuple_Test();
            var (first2, last2) = ReturnValueTuple_Test();
            (var first3, string last3) = ReturnValueTuple_Test();
        }

        private static void ClassDeconstruction_Test()
        {
            var p = new P(1, "lala");

            var (x, name) = p;

            var (x2, _) = p; //析构变量的数量必须和调用的析构方法保持一致

            Console.WriteLine($"{x},{name}");
        }

        private class P
        {
            public readonly string Name;

            public readonly int X;

            public P(int x, string name)
            {
                X = x;
                Name = name;
            }

            public void Deconstruct(out int x, out string name)
            {
                x = X;
                name = Name;
            }
        }
    }
}
