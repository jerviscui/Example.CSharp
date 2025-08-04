using BenchmarkDotNet.Running;

namespace MessagePackTest;

internal sealed class Program
{

    #region Constants & Statics

    private static void Main()
    {
        _ = BenchmarkRunner.Run<ArraySerializeBenchmark>();
        _ = BenchmarkRunner.Run<ArrayDeserializeBenchmark>();
        _ = BenchmarkRunner.Run<ClassSerializeBenchmark>();
        _ = BenchmarkRunner.Run<ClassDeserializeBenchmark>();
        _ = BenchmarkRunner.Run<StructSerializeBenchmark>();
        _ = BenchmarkRunner.Run<StructDeserializeBenchmark>();
    }

    #endregion

    private Program()
    {
    }
}
