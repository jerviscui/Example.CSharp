using System;

namespace SwitchExpressionTest
{
    internal sealed class Program
    {
        private enum Rainbow
        {
            Red = 0,

            Orange,

            Yellow,

            Green,

            Blue,

            Indigo,

            Violet
        }

        private static void Main(string[] args)
        {
            static int Lambda(Rainbow o) => o switch
            {
                Rainbow.Red => 0,
                Rainbow.Orange => 1,
                Rainbow.Yellow => 2,
                { } n when n > Rainbow.Green => 3, //greater than Green
                _ => -1                            //Green
            };

            foreach (var enumValue in typeof(Rainbow).GetEnumValues())
            {
                Console.WriteLine($"{(int)enumValue},{Lambda((Rainbow)enumValue)}");
            }

            Console.WriteLine();
            var a1 = new A { X = 1 };
            PropertyPattern_Test(a1);
            Console.WriteLine(a1.X);
            a1.Name = "a";
            PropertyPattern_Test(a1);
            Console.WriteLine(a1.Name);

            Console.WriteLine();
            Console.WriteLine(Tuple_Test("1", "2"));
            Console.WriteLine(Tuple_Test("2", "2"));
            Console.WriteLine(Tuple_Test("3", "3"));

            Console.WriteLine();
            Nested_Test();
        }

        private class A
        {
            public string Name { get; set; }

            public int X { get; set; }
        }

        private static void PropertyPattern_Test(A a)
        {
            var result = a switch
            {
                { X: 1 } => Exp1(a),
                { Name: "ina" } => Exp2(a),

                _ => null
            };

            A Exp1(A ina)
            {
                ina.X++;
                return ina;
            }

            A Exp2(A ina)
            {
                ina.Name = "aa";
                return ina;
            }
        }

        private static string Tuple_Test(string first, string second) => (first, second) switch
        {
            ("1", "2") => "first < second",
            ("2", "2") => "first = second",
            ("3", "2") => "first > second",
            _ => "none"
        };

        private class Aa : A
        {
            public int Y { get; set; }
        }

        private static void Nested_Test()
        {
            var a = new A() { X = 1 };
            var aa = new Aa { X = 2, Y = 3 };

            Console.WriteLine($"{Switch(a)}");
            Console.WriteLine($"{Switch(aa)}");

            int Switch(A ins) => ins switch
            {
                Aa inaa => inaa.Y switch
                {
                    3 => 3 * inaa.X,
                    4 => 1 + 2 + 3 + 4 + inaa.X,
                    _ => 0
                },

                A ina => a.X * 2,

                _ => -1
            };
        }
    }
}
