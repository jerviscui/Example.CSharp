using System.IO;    // IDE0005: Using directive is unnecessary


namespace CodeAnalysisTest // IDE0160 csharp_style_namespace_declarations
{
    using System;// ide0065 csharp_using_directive_placement

    internal sealed class Program
    {
        private static void Main(string[] args) // ide0210 csharp_style_prefer_top_level_statements
        {
            Console.WriteLine("Hello, World!");

            // ide0011 csharp_prefer_braces = true
            bool test = DateTime.Now.Ticks > 0;
            if (test != false)
                Console.WriteLine("Hello, World!");

            // ide0063 csharp_prefer_simple_using_statement = true
            using (var fileStream = new System.IO.FileStream("", FileMode.Open)) // ide0001
            {
                var canRead = fileStream.CanRead;
            }

        }

        private static void M1()
        {
            throw new NotSupportedException();
        }

        private static void M2(E e)
        {
            // IDE0002: 'C.M1' can be simplified to 'M1'
            Program.M1();

            // ide0004 Remove unnecessary cast
            int v = (int)0;

            // IDE0010: Add missing cases
            switch (e)
            {
                case E.A:
                    return;
                case E.B:
                default:
                    break;
            }

            // ide0017 otnet_style_object_initializer = true
            var my = new MyClass();
            my.Age = 21;

            // ide0028 dotnet_style_prefer_collection_expression = true 
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // IDE0029 dotnet_style_coalesce_expression = true
            var x = Random.Shared.Next() > 0 ? null : my;
            var y = my;
            var xy = x != null ? x : y;

            // IDE0270 dotnet_style_coalesce_expression = true
            var item = new object() as MyClass;
            if (item == null)
            {
                throw new InvalidOperationException();
            }
        }

        private enum E

        {
            A,
            B
        }
    }
}
