namespace StringFormatTest;

internal static partial class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        //ValueStringBuilderTest.Test();

        StringBuilderCacheTest.Test();

        //_ = BenchmarkRunner.Run<BenchmarkTest>();

        //var watch = new Stopwatch();
        //watch.Restart();
        //for (int i = 0; i < 1_000_000; i++)
        //{
        //    UnBoxTest();
        //}
        //watch.Stop();
        //Print.Microsecond(watch);

        //watch.Restart();
        //for (int i = 0; i < 1_000_000; i++)
        //{
        //    BoxTest();
        //}
        //watch.Stop();
        //Print.Microsecond(watch);
        //// 47,992 us
        ////150,643 us

        //Console.WriteLine(BoxFormatTest());
        //Console.WriteLine(UnBoxFormatTest());

        //var watch = new Stopwatch();
        //watch.Restart();
        //for (int i = 0; i < 1_000_000; i++)
        //{
        //    UnBoxFormatTest();
        //}
        //watch.Stop();
        //Print.Microsecond(watch);

        //watch.Restart();
        //for (int i = 0; i < 1_000_000; i++)
        //{
        //    BoxFormatTest();
        //}
        //watch.Stop();
        //Print.Microsecond(watch);
        ////233,697 us
        ////277,426 us
    }

    public static string BoxFormatTest()
    {
        var a = 1;
        return $"{a,10:##,###} xxx";

        //IL_0002: ldstr        "{0,10:##,###}"
        //IL_0007: ldloc.0      // a
        //IL_0008: box          [System.Runtime]System.Int32
        //IL_000d: call         string [System.Runtime]System.String::Format(string, object)

        //.Net 6 中使用 DefaultInterpolatedStringHandler 格式化字符串
    }

    public static string BoxTest()
    {
        var a = 1;

        return $"string {a}";

        //IL_0003: ldstr        "string {0}"
        //IL_0008: ldloc.0      // a
        //IL_0009: box          [System.Runtime]System.Int32
        //IL_000e: call         string [System.Runtime]System.String::Format(string, object)
    }

    public static string UnBoxFormatTest()
    {
        var a = 1;
#pragma warning disable IDE0071 // Simplify interpolation
        return $"{a.ToString("##,###")} xxx";
#pragma warning restore IDE0071 // Simplify interpolation

        //IL_0003: ldstr        "{0,10}"
        //IL_0008: ldloca.s     a
        //IL_000a: ldstr        "##,###"
        //IL_000f: call         instance string [System.Runtime]System.Int32::ToString(string)
        //IL_0014: call         string [System.Runtime]System.String::Format(string, object)
    }

    public static string UnBoxTest()
    {
        var a = 1;

        //编译器提示 IDE0071 简化内插
#pragma warning disable IDE0071 // Simplify interpolation
        return $"string {a.ToString()}";
#pragma warning restore IDE0071 // Simplify interpolation

        //IL_0003: ldstr        "string "
        //IL_0008: ldloca.s     a
        //IL_000a: call         instance string [System.Runtime]System.Int32::ToString()
        //IL_000f: call         string [System.Runtime]System.String::Concat(string, string)
    }

    #endregion

}
