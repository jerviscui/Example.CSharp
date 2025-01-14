using System;

namespace StringFormatTest;

public static class StringBuilderCacheTest
{

    #region Constants & Statics

    public static void Test()
    {
        var sbc = StringBuilderCache.Acquire();

        _ = sbc.Append("abc").Append('\t').Append("def");

        var str = StringBuilderCache.GetStringAndRelease(sbc);
        Console.WriteLine(str);
    }

    #endregion

}
