using System;

namespace CodeAnalysisTest;

internal sealed class PatternMatchingTest
{
    private static void M(object o)
    {
        // ide0019 csharp_style_pattern_matching_over_as_with_null_check = true
        var s = o as string;
        if (s != null)
        {
            Console.WriteLine(s);
        }

        // IDE0038 csharp_style_pattern_matching_over_is_with_cast_check = true
        if (o is int) { Console.WriteLine((int)o); }
    }
}
