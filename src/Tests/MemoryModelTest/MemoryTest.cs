using System;
using System.Threading.Tasks;

namespace MemoryModelTest;

public static class MemoryTest
{

    #region Constants & Statics

    public static async Task MemoryToSpanTestAsync()
    {
        await Task.Yield();

        Memory<int> memory = new int[5];
        var span = memory.Span;  // 高性能访问
        span[0] = 42;

        Console.WriteLine(span[0]);
    }

    #endregion

}
