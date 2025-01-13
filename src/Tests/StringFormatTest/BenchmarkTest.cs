using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace StringFormatTest;

[SimpleJob(RuntimeMoniker.Net50)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkTest
{

    #region Methods

    [Benchmark]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public void BoxFormatTest()
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
        _ = Program.BoxFormatTest();
    }

    //[Benchmark]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public void BoxTest()
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
        _ = Program.BoxTest();
    }

    [Benchmark]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public void UnBoxFormatTest()
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
        _ = Program.UnBoxFormatTest();
    }

    //[Benchmark]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public void UnBoxTest()
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
        _ = Program.UnBoxTest();
    }

    #endregion

}
