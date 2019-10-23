using System;

namespace SwitchExpressionTest
{
    class Program
    {
        public enum Rainbow
        {
            Red = 0,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet
        }

        static void Main(string[] args)
        {
            var c = Rainbow.Blue;

            Func<Rainbow, int> lambda = (o) => o switch
            {
                Rainbow.Red => 0,
                Rainbow.Orange => 1,
                Rainbow.Yellow => 2,
                Rainbow n when n > Rainbow.Green => 3, //greater than Green
                _ => -1 //Green
            };

            foreach (var enumValue in typeof(Rainbow).GetEnumValues())
            {
                Console.WriteLine($"{(int)enumValue},{lambda((Rainbow)enumValue)}");
            }

            Console.WriteLine();
            var a1 = new A() { X = 1 };
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

        class A
        {
            public string Name { get; set; }

            public int X { get; set; }
        }

        static void PropertyPattern_Test(A a)
        {
            var result = a switch
            {
                { X: 1 } => Exp1(a),
                { Name: "a" } => Exp2(a),

                _ => null
            };

            A Exp1(A a)
            {
                a.X++;
                return a;
            }

            A Exp2(A a)
            {
                a.Name = "aa";
                return a;
            }
        }

        static string Tuple_Test(string first, string second) => (first, second) switch
        {
            ("1", "2") => "first < second",
            ("2", "2") => "first = second",
            ("3", "2") => "first > second",
            _ => "none"
        };

        class AA : A
        {
            public int Y { get; set; }
        }

        static void Nested_Test()
        {
            var a = new A() { X = 1 };
            var aa = new AA() { X = 2, Y = 3 };

            Console.WriteLine($"{Switch(a)}");
            Console.WriteLine($"{Switch(aa)}");

            int Switch(A a) => a switch
            {
                AA x => x.Y switch
                {
                    3 => 3 * x.X,
                    4 => 1 + 2 + 3 + 4 + x.X,
                    _ => 0
                },

                A __ => __.X * 2,

                _ => -1
            };
        }
    }
}
