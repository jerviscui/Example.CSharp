using BenchmarkDotNet.Running;

namespace MessagePackTest;

internal sealed class Program
{

    #region Constants & Statics

    private static void Main()
    {
        _ = BenchmarkRunner.Run<ArraySerializeBenchmark>();
        _ = BenchmarkRunner.Run<ArrayDeserializeBenchmark>();
    }

    #endregion

    private Program()
    {
    }
}
