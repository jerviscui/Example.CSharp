using System.IO;    // IDE0005: Using directive is unnecessary


namespace CodeAnalysisTest // IDE0160 csharp_style_namespace_declarations
{
    using System;// ide0065 csharp_using_directive_placement
    using System.Diagnostics.CodeAnalysis;

    internal sealed class Program
    {
        private static void Main(string[] args) // ide0210 csharp_style_prefer_top_level_statements
        {
            // ide0011 csharp_prefer_braces = true
            bool test = DateTime.Now.Ticks > 0;
            if (test != false)
                Console.WriteLine("Hello, World!");

            // ide0063 csharp_prefer_simple_using_statement = true
            using (var fileStream = new System.IO.FileStream("", FileMode.Open)) // ide0001 Name canbesimplified
            {
                var canRead = fileStream.CanRead;
            }

            // ide0062 csharp_prefer_static_local_function = false
            Hello();
            void Hello() => Console.WriteLine("Hello");

            // ide1005 csharp_style_conditional_delegate_call = false
            if (func != null)
            {
                func(args);
            }

            // ide0048 dotnet_style_parentheses_in_arithmetic_binary_operators = always_for_clarity
            var v = 1 + 1 * 1;
            Console.WriteLine(v);
            // ide0048 dotnet_style_parentheses_in_relational_binary_operators = always_for_clarity
            var vv = 1 < 2 == 1 > 3;
            Console.WriteLine(vv);
            // ide0048 dotnet_style_parentheses_in_other_binary_operators = always_for_clarity
            var vvv = true || true && false;
            Console.WriteLine(vvv);
            // ide0047 dotnet_style_parentheses_in_other_operators = never_if_unnecessary
            var myStruct = new MyStruct();
            var vvvv = (myStruct.Value).Length;
            Console.WriteLine(vvvv);
        }

        private static Action<string[]>? func = args => { };

        // ide0060 dotnet_code_quality_unused_parameters = all
        private static int Test(int unusedParam) { return 1; }
        internal static int GetNum2(int unusedParam) { return 1; }
        private static int GetNum3(int unusedParam) { return 1; }

        // ide0280 Use nameof
        private static int M([NotNullIfNotNull("input")] string? input) { return 1; }
    }

    // ide0040 dotnet_style_require_accessibility_modifiers = always
    sealed class ModifierClass
    {
        // ide0044 dotnet_style_readonly_field = true 
        private int _daysInYear = 365;

        // ide0049 dotnet_style_predefined_type_for_locals_parameters_members = true
        private Int32 _member;

        private static int M()
        {
            // ide0049 dotnet_style_predefined_type_for_member_access = true
            var local = Int32.MaxValue;

            return local;
        }

        // ide0036 csharp_preferred_modifier_order = public,private,protected,internal,file,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,required,volatile,async
        private readonly static int Days = 365;
    }


    struct MyStruct
    {
        public MyStruct()
        {
            Value = "";
        }

        // ide0064 Make struct fields writable
        //public readonly string Value;
        public string Value;

        public MyStruct(string value)
        {
            Value = value;
        }

        public void Test()
        {
            this = new MyStruct("");
        }
    }

    // ide0250 csharp_style_prefer_readonly_struct = true
    struct S
    {
        readonly int i = 1;

        public S(int i)
        {
            this.i = i;
        }

        // ide0251 csharp_style_prefer_readonly_struct_member = true
        int M()
        {
            return i;
        }
    }


}


