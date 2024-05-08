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

    }
}
