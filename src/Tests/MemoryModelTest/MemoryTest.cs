using System;
using System.Buffers;
using System.Threading.Tasks;

namespace MemoryModelTest;

public static class MemoryTest
{

    #region Constants & Statics

    public static async Task MemoryPoolTestAsync()
    {
        await Task.Yield();

        var owner = MemoryPool<byte>.Shared.Rent(1);

        var memory = owner.Memory;
        memory.Span[0] = 84;

        Console.WriteLine(string.Join(' ', memory.ToArray()));
        //output:84 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0

        owner.Dispose();
    }

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
