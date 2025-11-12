namespace StructTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        //var m1 = new Measurement();
        //Console.WriteLine(m1);  // output: NaN (Undefined)

        //var m2 = default(Measurement);
        //Console.WriteLine(m2);  // output: 0 ()

        //var ms = new Measurement[2];
        //Console.WriteLine(string.Join(", ", ms));  // output: 0 (), 0 ()

        //ValueTypeTest.ShowSize();

        //ValueTypeTest.OutOfPrecision_True_Test();
        //ValueTypeTest.OutOfPrecision_True2_Test();
        //ValueTypeTest.OutOfPrecision_False_Test();

        //Console.WriteLine();
        //ValueTypeTest.EffectiveLength_4();

        //Console.WriteLine();
        //ValueTypeTest.RoundAlgorithm();

        //ValidationTest.ValidationError();
        //Console.WriteLine();
        //ValidationTest.ValidationAllow();

        StructTest.Test();

        //var config = DefaultConfig.Instance
        //    .WithArtifactsPath(
        //        $".\\BenchmarkDotNet.Aritfacts.{DateTime.Now.ToString("u").Replace(' ', '_').Replace(':', '-')}")
        //    .AddExporter(MarkdownExporter.GitHub)
        //    .AddDiagnoser(MemoryDiagnoser.Default)
        //    .AddJob(Job.MediumRun.WithLaunchCount(1));

        //_ = BenchmarkRunner.Run<ClassBenchmark>(config);
    }

    #endregion

}
