using System;

namespace CodeAnalysisTest;

internal sealed class PatternMatchingTest
{
    private static int M(object o)
    {
        // ide0019 csharp_style_pattern_matching_over_as_with_null_check = true
        var s = o as string;
        if (s != null)
        {
            Console.WriteLine(s);
        }

        // IDE0038 csharp_style_pattern_matching_over_is_with_cast_check = true
        if (o is int) { Console.WriteLine((int)o); }

        // ide0066 csharp_style_prefer_switch_expression = true
        int x = Random.Shared.Next();
        int i;
        switch (x)
        {
            case 1:
                i = 1 * 1;
                break;
            case 2:
                i = 2 * 2;
                break;
            default:
                i = 0;
                break;
        }

        // IDE0078 csharp_style_prefer_pattern_matching = false
        //var b = !(o is PatternMatchingTest p);

        // IDE0260 csharp_style_pattern_matching_over_as_with_null_check = true
        if ((o as string)?.Length == 0)
        {
            return 1;
        }

        return 0;
    }

    public sealed record Point(int X, int Y);
    public sealed record Segment(Point Start, Point End);

    // ide0170 Simplify property pattern
    static bool IsEndOnXAxis(Segment segment) =>
        segment is { Start: { Y: 0 } } or { End: { Y: 0 } };
}
