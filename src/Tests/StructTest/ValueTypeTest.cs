namespace StructTest;

public static class ValueTypeTest
{

    #region Constants & Statics

    public static void EffectiveLength_4()
    {
        var a = 999999.9999;
        var b = 999999999.9999;
        var c = 999999999999.9999;
        var e = 999999999999999.9999;
        var f = 999999999999999999.9999;

        var ee = 999999999999999.9999m;
        var ff = 999999999999999999.9999m;

        Console.WriteLine($"double 6+4 {a}");
        Console.WriteLine($"double 9+4 {b}");
        Console.WriteLine($"double 12+4 {c}");
        Console.WriteLine($"double 15+4 {e}"); // 无效，15 位整数部分 + 4 位小数
        Console.WriteLine($"double 18+4 {f}"); // 无效，18 位整数部分 + 4 位小数

        Console.WriteLine($"decimal 15+4 {ee}");
        Console.WriteLine($"decimal 18+4 {ff}");
    }

    public static void OutOfPrecisionFalseTest()
    {
        //（15 - 16 位有效数字）
        var maxValue = 999_999_999_999.9999; // 12 位整数部分 + 4 位小数
        var beyondMax = 999_999_999_999.9998;

        Console.WriteLine($"Max value with 4 decimals: {maxValue}");
        Console.WriteLine($"Beyond max value with 4 decimals: {beyondMax}");
        Console.WriteLine($"Are maxValue and beyondMax equal? {maxValue == beyondMax}");
    }

    public static void OutOfPrecisionTrueTest()
    {
        //（15 - 16 位有效数字）
        var maxValue = 1_999_999_999_999.9999; // 超过 12 位整数部分 + 4 位小数
        var beyondMax = 2_000_000_000_000;

        Console.WriteLine($"Max value with 4 decimals: {maxValue}");
        Console.WriteLine($"Beyond max value with 4 decimals: {beyondMax}");
        Console.WriteLine($"Are maxValue and beyondMax equal? {maxValue == beyondMax}");
    }

    public static void RoundAlgorithm()
    {
        // 银行家舍入法(四舍六入五成双)
        var rounded = Math.Round(1.235m, 2, MidpointRounding.ToEven);

        // 会计常用舍入(总是远离零)
        var accountingRound = Math.Round(1.235m, 2, MidpointRounding.AwayFromZero);

        Console.WriteLine(rounded);
        Console.WriteLine(accountingRound);
    }

    public static void ShowSize()
    {
        Console.WriteLine($"sbyte: Min={sbyte.MinValue}, Max={sbyte.MaxValue}, Size={sizeof(sbyte)} bytes");
        Console.WriteLine($"byte: Min={byte.MinValue}, Max={byte.MaxValue}, Size={sizeof(byte)} bytes");
        Console.WriteLine($"short: Min={short.MinValue}, Max={short.MaxValue}, Size={sizeof(short)} bytes");
        Console.WriteLine($"ushort: Min={ushort.MinValue}, Max={ushort.MaxValue}, Size={sizeof(ushort)} bytes");
        Console.WriteLine($"int: Min={int.MinValue}, Max={int.MaxValue}, Size={sizeof(int)} bytes");
        Console.WriteLine($"uint: Min={uint.MinValue}, Max={uint.MaxValue}, Size={sizeof(uint)} bytes");
        Console.WriteLine($"long: Min={long.MinValue}, Max={long.MaxValue}, Size={sizeof(long)} bytes");
        Console.WriteLine($"ulong: Min={ulong.MinValue}, Max={ulong.MaxValue}, Size={sizeof(ulong)} bytes");
        Console.WriteLine($"float: Min={float.MinValue}, Max={float.MaxValue}, Size={sizeof(float)} bytes");
        Console.WriteLine($"double: Min={double.MinValue}, Max={double.MaxValue}, Size={sizeof(double)} bytes");
        Console.WriteLine($"decimal: Min={decimal.MinValue}, Max={decimal.MaxValue}, Size={sizeof(decimal)} bytes");
    }

    #endregion

}
