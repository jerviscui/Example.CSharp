using Common;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MemoryModelTest;

[SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "<Pending>")]
public static class ArrayPoolTest
{

    #region Constants & Statics

    private static readonly ArrayPool<byte> Pool1 = ArrayPool<byte>.Create();

    private static byte[] GetArray(int length, bool usePool)
    {
        if (usePool)
        {
            return Pool1.Rent(length);
        }

        return new byte[length];
    }

    public static async Task Array_Test()
    {
        for (var i = 0; i < 10; i++)
        {
            var j = i;

#pragma warning disable CS4014
            Task.Run(
                async () =>
#pragma warning restore CS4014
                {
                    var random = new Random(j);
                    while (true)
                    {
                        var bytes = GetArray(1_000_000 + random.Next(100_000), false);
                        await Task.Delay(10);
                    }
                });
        }

        while (true)
        {
            Print.MemoryInfo();
            await Task.Delay(1000);
        }
    }

    public static async Task ArrayPool_Test()
    {
        for (var i = 0; i < 10; i++)
        {
            var j = i;

#pragma warning disable CS4014
            Task.Run(
                async () =>
#pragma warning restore CS4014
                {
                    var random = new Random(j);
                    while (true)
                    {
                        var bytes = GetArray(1_000_000 + random.Next(100_000), true);
                        await Task.Delay(10);
                        Pool1.Return(bytes);
                    }
                });
        }

        while (true)
        {
            Print.MemoryInfo();
            await Task.Delay(1000);
        }
    }

    #endregion

}
