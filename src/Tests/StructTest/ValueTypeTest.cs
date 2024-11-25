namespace StructTest;

public class ValueTypeTest
{

    #region Constants & Statics

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
        var maxValue = 1_999_999_999_999.9999;
        var beyondMax = 2_000_000_000_000; // 超过 12 位整数部分 + 4 位小数

        Console.WriteLine($"Max value with 4 decimals: {maxValue}");
        Console.WriteLine($"Beyond max value with 4 decimals: {beyondMax}");
        Console.WriteLine($"Are maxValue and beyondMax equal? {maxValue == beyondMax}");
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
