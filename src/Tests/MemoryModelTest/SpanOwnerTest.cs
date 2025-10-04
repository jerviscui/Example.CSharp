using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Threading.Tasks;

namespace MemoryModelTest;

public static class SpanOwnerTest
{

    #region Constants & Statics

    public static async Task Test()
    {
        await Task.Yield();

        using var buffer = SpanOwner<int>.Allocate(1);

        var span = buffer.Span;
        span[0] = 84;

        Console.WriteLine(string.Join(' ', span.ToArray()));
        //output: 84
    }

    #endregion

}
