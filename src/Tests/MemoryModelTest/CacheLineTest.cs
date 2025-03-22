using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryModelTest;

public static class CacheLineTest
{

    #region Constants & Statics

    private static readonly int NUM_INTS = 50_000_000;

    public static void SharedCacheLine_Test()
    {
        var stopwatch = Stopwatch.StartNew();

        var ng1 = new NumberGenerator();
        var ng2 = new NumberGenerator();
        var numbers1 = new int[NUM_INTS];
        var numbers2 = new int[NUM_INTS];

        //Generate numbers in parallel with a high probability of sharing the cache line
        Parallel.Invoke(
            () =>
            {
                for (var i = 0; i < NUM_INTS; i++)
                {
                    numbers1[i] = ng1.Generate();
                }
            },
            () =>
            {
                for (var i = 0; i < NUM_INTS; i++)
                {
                    numbers2[i] = ng2.Generate();
                }
            });

        Console.WriteLine($"numbers1:{numbers1.Max()}");
        Console.WriteLine($"numbers2:{numbers2.Max()}");
        // 2364
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    public static void ExcCacheLine_Test()
    {
        var stopwatch = Stopwatch.StartNew();

        var numbers1 = Array.Empty<int>();
        var numbers2 = Array.Empty<int>();

        Parallel.Invoke(
            () =>
            {
                var ngl = new NumberGenerator();
                numbers1 = new int[NUM_INTS];

                for (var i = 0; i < NUM_INTS; i++)
                {
                    numbers1[i] = ngl.Generate();
                }
            },
            () =>
            {
                var ng2 = new NumberGenerator();
                numbers2 = new int[NUM_INTS];

                for (var i = 0; i < NUM_INTS; i++)
                {
                    numbers2[i] = ng2.Generate();
                }
            });

        Console.WriteLine($"numbers1:{numbers1.Max()}");
        Console.WriteLine($"numbers2:{numbers2.Max()}");
        // 1208, 时间快很多
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    #endregion

}

internal class NumberGenerator
{
    private int _number;

    #region Methods

    public int Generate()
    {
        _number++;
        return _number;
    }

    #endregion
}
