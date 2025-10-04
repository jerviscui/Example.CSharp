using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Threading.Tasks;

namespace MemoryModelTest;

public static class MemoryOwnerTest
{

    #region Constants & Statics

    public static async Task AllocateLengthTestAsync()
    {
        await Task.Yield();

        using var buffer = MemoryOwner<int>.Allocate(1);

        var memory = buffer.Memory;
        memory.Span[0] = 84;

        Console.WriteLine(string.Join(' ', memory.ToArray()));
        //output: 84
    }

    public static void SliceTest()
    {
        using var buffer = MemoryOwner<int>.Allocate(42);
        if (buffer.Length > 50)
        {
            using var buffer2 = buffer[..50];

            var span = buffer2.Span;
            span[0] = 123;
        }
    }

    #endregion

}
